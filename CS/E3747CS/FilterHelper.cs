using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Data;
using System.Data;
using System.ComponentModel;
using DevExpress.Data.Filtering.Helpers;
using System.Collections;

namespace FilterMasterDetailGrid
{
    public class FilterHelper
    {
        GridView _parentView;
        GridView _childView;

        public FilterHelper(GridView parentView, GridView childView)
        {
            _parentView = parentView;
            _parentView.CustomRowFilter += _view_CustomRowFilter;
            _childView = childView;
        }

        void _view_CustomRowFilter(object sender, DevExpress.XtraGrid.Views.Base.RowFilterEventArgs e)
        {
            GridView view = (sender as GridView);
            BaseGridController controller = view.DataController;
            PropertyDescriptorCollection pdc = null;
                IList source = (IList)view.DataSource;
            if (view.DataSource is ITypedList)
            {
                pdc = (view.DataSource as ITypedList).GetItemProperties(null);
            }
            else
            {
                pdc = TypeDescriptor.GetProperties(source.GetType().GetProperty("Item").PropertyType);
            }
            ExpressionEvaluator ev = new ExpressionEvaluator(pdc, controller.FilterCriteria);
            e.Visible = !IsEmptyDetail(e.ListSourceRow, controller) && ev.Fit(source[e.ListSourceRow]);
            e.Handled = true;
        }


        private bool IsEmptyDetail(int listSourceRow, BaseGridController controller)
        {
            IList detail = (IList)controller.GetDetailList(listSourceRow, 0);
            PropertyDescriptorCollection pdc = null;
            if (detail is ITypedList)
            {
                pdc = (detail as ITypedList).GetItemProperties(null);
            }
            else
            {
                pdc = TypeDescriptor.GetProperties(detail.GetType().GetProperty("Item").PropertyType);
            }
            ExpressionEvaluator detailEvaluator = new ExpressionEvaluator(pdc, _childView.DataController.FilterCriteria);
            foreach (object o in detail)
            {
                if (detailEvaluator.Fit(o))
                    return false;
            }
            return true;
        }
    }
}
