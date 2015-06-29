using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TechSupport_Utilities
{
    /*
    CREATE TABLE [dbo].[utils_categories](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[cat_id] [int] NOT NULL,
	[name] [nchar](130) NOT NULL,
	[description] [ntext] NOT NULL,
	[util_ids] [nchar](80) NOT NULL,
     CONSTRAINT [PK_utils_categories] PRIMARY KEY CLUSTERED 
    (
	    [id] ASC
    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

    GO

    ALTER TABLE [dbo].[utils_categories] ADD  CONSTRAINT [DF_utils_categories_util_ids]  DEFAULT ((0)) FOR [util_ids]
    GO
    */
    public class category
    {
        [XmlElement("cat_id")]
        public int cat_id { get; set; }
        [XmlElement("name")]
        public string name { get; set; }
        [XmlElement("description")]
        public string description { get; set; }

        /// <summary>
        /// a list of util_id values providing a tool for the category
        /// </summary>
        [XmlElement("util_ids")]
        public string util_ids { get; set; }

        public category()
        {
            cat_id = -1;
            name = "undef";
            description = "placeholder";
            util_ids = "-1 ";
        }
    }
}
