using Microsoft.AspNetCore.Mvc.Rendering;

namespace TestApp.Helper
{
    public static class Utils
    {
        public static List<SelectListItem> RecordListItems()
        {
            List<SelectListItem> items = new List<SelectListItem>
            {
                new SelectListItem {Text = "5", Value = "5"},
                new SelectListItem {Text = "10", Value = "10"},
                new SelectListItem {Text = "25", Value = "25"},
                new SelectListItem {Text = "50", Value = "50"},
                new SelectListItem {Text = "100", Value = "100"}
            };
            return items;
        }

    }
}
