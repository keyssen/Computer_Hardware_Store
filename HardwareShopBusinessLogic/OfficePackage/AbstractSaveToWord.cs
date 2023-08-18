using HardwareShopBusinessLogic.OfficePackage.HelperEnums;
using HardwareShopBusinessLogic.OfficePackage.HelperModels;

namespace HardwareShopBusinessLogic.OfficePackage
{
    public abstract class AbstractSaveToWord
    {
        public void CreateBuildGoodReport(WordInfo info)
        {
            CreateWord(info);

            CreateParagraph(new WordParagraph
            {
                Texts = new List<(string, WordTextProperties)> { (info.Title, new WordTextProperties { Bold = true, Size = "24" }) },
                TextProperties = new WordTextProperties
                {
                    Size = "24",
                    JustificationType = WordJustificationType.Center
                }
            });

            List<WordRow> rows = new List<WordRow>();
            rows.Add(new WordRow
            {
                Rows = new List<(string, WordTextProperties)> {
                        ("Товары", new WordTextProperties { Size = "24", Bold = true }),
                        ("Сборки", new WordTextProperties { Size = "24", Bold = true })
                }
            });

            var reportRecords = info.BuildGood;
            foreach (var reportRecord in reportRecords)
            {
                rows.Add(new WordRow
                {
                    Rows = new List<(string, WordTextProperties)> 
                    {
                        (reportRecord.GoodName, new WordTextProperties { Size = "24" }),
                        ("", new WordTextProperties { })
                    }
                });
                for (int i = 0; i < reportRecord.Builds.Count; i++)
                {
                    rows.Add(new WordRow
                    {
                        Rows = new List<(string, WordTextProperties)>
                        {
                            ("", new WordTextProperties { }),
                            (reportRecord.Builds[i], new WordTextProperties { Size = "24" })
                        }
                    });
                }
            }

            CreateTable(rows);

            SaveWord(info);
        }

		public void CreateBuildPurchaseReport(WordInfo info)
		{
			CreateWord(info);

			CreateParagraph(new WordParagraph
			{
				Texts = new List<(string, WordTextProperties)> { (info.Title, new WordTextProperties { Bold = true, Size = "24" }) },
				TextProperties = new WordTextProperties
				{
					Size = "24",
					JustificationType = WordJustificationType.Center
				}
			});

			List<WordRow> rows = new List<WordRow>();
			rows.Add(new WordRow
			{
				Rows = new List<(string, WordTextProperties)> {
						("Покупки", new WordTextProperties { Size = "24", Bold = true }),
						("Сборки", new WordTextProperties { Size = "24", Bold = true }),
						("Количество", new WordTextProperties { Size = "24", Bold = true }),
						("Компоненты", new WordTextProperties { Size = "24", Bold = true }),
						("Количество", new WordTextProperties { Size = "24", Bold = true })
				}
			});

			var reportRecords = info.PurchaseComponent;
			foreach (var reportRecord in reportRecords)
			{
				rows.Add(new WordRow
				{
					Rows = new List<(string, WordTextProperties)>
					{
						(reportRecord.Id.ToString(), new WordTextProperties { }),
						("", new WordTextProperties { }),
						("", new WordTextProperties { }),
						("", new WordTextProperties { }),
						("", new WordTextProperties { })
					}
				});
				for (int i = 0; i < reportRecord.Builds.Count; i++)
				{
					rows.Add(new WordRow
					{
						Rows = new List<(string, WordTextProperties)>
						{
							("", new WordTextProperties { }),
							(reportRecord.Builds[i].Build, new WordTextProperties { }),
							(reportRecord.Builds[i].count.ToString(), new WordTextProperties { }),
							("", new WordTextProperties { }),
							("", new WordTextProperties { })
						}
					});
					for(int j = 0; j < reportRecord.Builds[i].Item3.Count; j++)
					{
						rows.Add(new WordRow
						{
							Rows = new List<(string, WordTextProperties)>
							{
								("", new WordTextProperties { }),
								("", new WordTextProperties { }),
								("", new WordTextProperties { }),
								(reportRecord.Builds[i].Item3[j].Component, new WordTextProperties { }),
								(reportRecord.Builds[i].Item3[j].count.ToString(), new WordTextProperties { })
							}
						});
					}
				}
			}

			CreateTable(rows);

			SaveWord(info);
		}

		/// <summary>
		/// Создание doc-файла
		/// </summary>
		/// <param name="info"></param>
		protected abstract void CreateWord(WordInfo info);

        /// <summary>
        /// Создание таблицы
        /// </summary>
        /// <param name="rows"></param>
        protected abstract void CreateTable(List<WordRow> rows);

        /// <summary>
        /// Создание абзаца с текстом
        /// </summary>
        /// <param name="paragraph"></param>
        /// <returns></returns>
        protected abstract void CreateParagraph(WordParagraph paragraph);

        /// <summary>
		/// Сохранение файла
		/// </summary>
		/// <param name="info"></param>
        protected abstract void SaveWord(WordInfo info);
    }
}