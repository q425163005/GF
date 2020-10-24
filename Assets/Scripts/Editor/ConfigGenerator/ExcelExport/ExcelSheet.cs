using System;
using System.Collections.Generic;
using System.Data;

namespace ExcelExport
{
	// Token: 0x0200003C RID: 60
	public class ExcelSheet
	{
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600012B RID: 299 RVA: 0x0001157C File Offset: 0x0000F77C
		public string FullName
		{
			get
			{
				return string.Concat(new string[]
				{
					"[",
					this.FileName,
					"(",
					this.Name,
					")]"
				});
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600012C RID: 300 RVA: 0x000115B3 File Offset: 0x0000F7B3
		public string ConfigName
		{
			get
			{
				return this.Name + "Config";
			}
		}

		// Token: 0x0400013A RID: 314
		public string Name;

		// Token: 0x0400013B RID: 315
		public string NameDes;

		// Token: 0x0400013C RID: 316
		public string Interface;

		// Token: 0x0400013D RID: 317
		public bool IsVert = false;

		// Token: 0x0400013E RID: 318
		public string Export = string.Empty;

		// Token: 0x0400013F RID: 319
		public bool IsEncrypt = false;

		// Token: 0x04000140 RID: 320
		public List<ExcelSheetTableField> Fields = new List<ExcelSheetTableField>();

		// Token: 0x04000141 RID: 321
		public DataTable Table;

		// Token: 0x04000142 RID: 322
		public string FileName;
	}
}
