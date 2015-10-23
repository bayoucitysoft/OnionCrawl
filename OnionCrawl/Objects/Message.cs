using OnionCrawl.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionCrawl.Objects
{
    public class Message
    {
        public long Id { get; set; }
        public string Contents { get; set; }
        public DateTime Created { get; set; }

        public Guid Guid { get; set; }
        public long NarrativeId { get; set; }
        public bool HasErrors { get; set; }

        public Message(long narrativeId, bool error, string contents)
        {
            Created = DateTime.Now;
            Guid = Guid.NewGuid();
            NarrativeId = narrativeId;
            HasErrors = error;
            Contents = contents;
        }

        internal void UploadNarrativeMessage()
        {
            SQLAccess db = new SQLAccess();
            db.Procedure = "InsertNarrativeMessage";
            db.Parameters.Add(@"@contents", Contents);
            db.Parameters.Add(@"@created", Created);
            db.Parameters.Add(@"@guid", Guid);          
            db.Parameters.Add(@"@errors", HasErrors);
            db.Parameters.Add(@"@narrative_id", NarrativeId);
            db.ExecuteProcedure();
        }
    }
}
