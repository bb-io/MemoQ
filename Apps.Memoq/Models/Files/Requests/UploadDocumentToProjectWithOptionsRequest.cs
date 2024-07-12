using Apps.Memoq.Models.ServerProjects.Requests;
using Apps.MemoQ.DataSourceHandlers;
using Apps.MemoQ.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.MemoQ.Models.Files.Requests
{
    public class UploadDocumentToProjectWithOptionsRequest : ProjectRequest
    {
        [Display("Documents")]
        public FileReference File { get; set; }

        [Display("Target languages"), StaticDataSource(typeof(TargetLanguageDataHandler))]
        public IEnumerable<string>? TargetLanguageCodes { get; set; }

        [Display("File name")]
        public string? FileName {get; set;}

        [Display("Preview creation"), StaticDataSource(typeof(PreviewCreationDataHandler))]
        public string? PreviewCreation { get; set; }

        [Display("External document ID", Description = "An identifier of this document that is stored by memoQ. The value is not interpreted, only stored and can be used for delivery.")]
        public string? ExternalDocumentId { get; set; }

        [Display("Filter config GUID")]
        [DataSource(typeof(FilterConfigDataHandler))]
        public string? FilterConfigResGuid { get; set; }

        [Display("Import embedded images", Description = "Determines whether to import embedded images from the file. The images will appear as translatable documents. Embedded images can be imported from Office Open XML documents (docx, pptx, xlsx).")]
        public bool? ImportEmbeddedImages { get; set; }

        [Display("Import embedded objects", Description = "Determines whether to import embedded objects from the file. Embedded objects are for example an embedded Excel spreadsheet in a Word document. Supported embedded objects will appear as translatable documents. Embedded objects can be imported from Office Open XML documents (docx, pptx, xlsx). If the specified filter configuration does not specify the configuration of embedded objects, they will be imported with default configurations.")]
        public bool? ImportEmbeddedObjects { get; set; }

    }
}
