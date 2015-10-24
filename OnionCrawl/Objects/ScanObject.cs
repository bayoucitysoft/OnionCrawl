using OnionCrawl.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionCrawl.Objects
{
    public class ScanObject
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string PageStatus { get; set; }
        public string PageSource { get; set; }
        public int CrawlDepth { get; set; }

        private byte[] _screenshot;
        public byte[] screenshot { get; set; }
        /// <summary>
        /// finish initializing this 
        /// </summary>
        private Image screenshotImage;
        public Image Screenshot { get; set; }

        public long ParentId { get; set; }
        public ScanObject Parent { get; set; }
        public Guid Guid { get; set; }

        public ScanObject()
        {
            Guid = Guid.NewGuid();
        }

        public ScanObject(DataRow r)
        {
            if (!DBNull.Value.Equals(r["id"]))
                Id = (long)r["id"];
            if (!DBNull.Value.Equals(r["name"]))
                Name = (string)r["name"];
            if (!DBNull.Value.Equals(r["description"]))
                Description = (string)r["description"];
            if (!DBNull.Value.Equals(r["url"]))
                Url = (string)r["url"];
            if (!DBNull.Value.Equals(r["page_status"]))
                PageStatus = (string)r["page_status"];
            if (!DBNull.Value.Equals(r["page_source"]))
                PageSource = (string)r["page_source"];
            if (!DBNull.Value.Equals(r["crawl_depth"]))
                CrawlDepth = (int)r["crawl_depth"];
            if (!DBNull.Value.Equals(r["screenshot"]))
                screenshot = (byte[])(r["screenshot"]);
            if (!DBNull.Value.Equals(r["parent_id"]))
                ParentId = (long)r["parent_id"];
            if (!DBNull.Value.Equals(r["guid"]))
                Guid = (System.Guid)r["guid"];
        }

        public ScanObject(string name, string url, long parentId, int crawldepth)
        {
            Name = name;
            Url = url;
            ParentId = parentId;
            CrawlDepth = crawldepth;
            Guid = Guid.NewGuid();
        }

        public ScanObject FindById(long id)
        {
            SQLAccess db = new SQLAccess();
            db.Procedure = "ScanObjectById";
            db.Parameters.Add(@"@id", id);
            db.ExecuteProcedure();
            if (db.HasData)
                return new ScanObject(db.Response.Rows[0]);
            else return new ScanObject();
        }

            internal ScanObject GetNext()
        {
            SQLAccess db = new SQLAccess();
            db.Procedure = "NextScanObject";
            db.ExecuteProcedure();
            if (db.HasData)
                return new ScanObject(db.Response.Rows[0]);
            else return new ScanObject();
        }

        public void Update()
        {
            SQLAccess db = new SQLAccess();
            db.Procedure = "UpdateScanObject";
            db.Parameters.Add(@"@id", Id);
            db.Parameters.Add(@"@name", Name);
            db.Parameters.Add(@"@description", Description);
            db.Parameters.Add(@"@url", Url);
            db.Parameters.Add(@"@page_status", PageStatus);
            db.Parameters.Add(@"@page_source", PageSource);
            db.Parameters.Add(@"@crawl_depth", CrawlDepth);
            //db.Parameters.Add(@"@screenshot", screenshot);
            db.Parameters.Add(@"@parent_id", ParentId);
            db.ExecuteProcedure();
        }

        internal void Insert()
        {
            SQLAccess db = new SQLAccess();
            db.Procedure = "InsertScanObject";
            db.Parameters.Add(@"@name", Name);
            db.Parameters.Add(@"@description", Description);
            db.Parameters.Add(@"@url", Url);
            db.Parameters.Add(@"@page_status", PageStatus);
            db.Parameters.Add(@"@page_source", PageSource);
            db.Parameters.Add(@"@crawl_depth", CrawlDepth);
            db.Parameters.Add(@"@screenshot", screenshot);
            db.Parameters.Add(@"@parent_id", ParentId);
            db.Parameters.Add(@"@guid", Guid);
            db.ExecuteProcedure();
        }

    
    }
}
