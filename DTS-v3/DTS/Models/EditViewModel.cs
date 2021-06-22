namespace DTS.Models
{
    using System.Web.Mvc;

    public class EditViewModel
    {
        public Activities Activity { get; set; }
        public SelectList Categories { get; set; }
        public int SelectedParentActivityId { get; set; }
    }
}