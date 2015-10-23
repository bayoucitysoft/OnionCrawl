﻿using OnionCrawl.Utility;
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
        public byte[] screenshot
        {
            get
            {
                if (Screenshot != null)
                    using (MemoryStream ms = new MemoryStream())
                    {
                        Screenshot.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        return ms.ToArray();
                    }
                else return new byte[0];
            }
            set { _screenshot = value; }

        }
        /// <summary>
        /// finish initializing this 
        /// </summary>
        private Image screenshotImage;
        public Image Screenshot { get; set; }

        public ScanObject Parent { get; set; }

        public ScanObject()
        {

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
            if (!DBNull.Value.Equals(r["screenshot"]))
                screenshot = (byte[])(r["screenshot"]);
            if (!DBNull.Value.Equals(r["parent_id"]))
                Parent = new ScanObject().FindById((long)r["parent_id"]);
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


    }
}