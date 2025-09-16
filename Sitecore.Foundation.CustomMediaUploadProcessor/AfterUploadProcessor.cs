using System;
using Sitecore.Data.Items;
using Sitecore.Pipelines.Upload;
using Sitecore.SecurityModel;

namespace Sitecore.Foundation.CustomMediaUploadProcessor
{
    public class AfterUploadProcessor
    {
        public void Process(UploadArgs args)
        {
            if (args == null || args.Files == null || args.Files.Count == 0)
                return;

            // Read the date-only value you added to the upload dialog (input name="UploadDate")
            var dateStr = System.Web.HttpContext.Current?.Request?["UploadDate"];
            DateTime dateOnly;
            var hasDate = DateTime.TryParse(dateStr, out dateOnly);

            if (args.UploadedItems == null || args.UploadedItems.Count == 0)
                return; 
            var uploadedItem = args.UploadedItems[0];

            string isoDate = dateOnly.ToString("yyyyMMddTHHmmss");

            using (new SecurityDisabler())
            {
                uploadedItem.Editing.BeginEdit();
                try
                {
                    // replace "Event Date" with your actual field name or ID
                    uploadedItem.Fields["File Date"].Value = isoDate;
                    uploadedItem.Editing.EndEdit();
                }
                catch
                {
                    uploadedItem.Editing.CancelEdit();
                    throw;
                }
            }

        }
    }
}