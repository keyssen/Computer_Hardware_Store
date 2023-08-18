using HardwareShopBusinessLogic.OfficePackage.HelperEnums;
using HardwareShopBusinessLogic.OfficePackage.HelperModels;

namespace HardwareShopBusinessLogic.OfficePackage
{
    public abstract class AbstractSaveToExcel
    {
        /// <summary>
        /// Создание отчета по сборкам в выбранных товарах
        /// </summary>
        /// <param name="info"></param>
        public void CreateBuildGoodReport(ExcelInfo info)
        {
            CreateExcel(info);

            InsertCellInWorksheet(new ExcelCellParameters
            {
                ColumnName = "A",
                RowIndex = 1,
                Text = info.Title,
                StyleInfo = ExcelStyleInfoType.Title
            });

            MergeCells(new ExcelMergeParameters
            {
                CellFromName = "A1",
                CellToName = "B1"
            });

            uint rowIndex = 2;
            foreach (var bg in info.BuildGood)
            {
                InsertCellInWorksheet(new ExcelCellParameters
                {
                    ColumnName = "A",
                    RowIndex = rowIndex,
                    Text = bg.GoodName,
                    StyleInfo = ExcelStyleInfoType.TextWithBroder
                });
                rowIndex++;

                foreach (var build in bg.Builds)
                {
                    InsertCellInWorksheet(new ExcelCellParameters
                    {
                        ColumnName = "B",
                        RowIndex = rowIndex,
                        Text = build,
                        StyleInfo = ExcelStyleInfoType.TextWithBroder
                    });

                    rowIndex++;
                }
            }

            SaveExcel(info);
        }

		/// <summary>
		/// Создание отчета по сборкам в выбранных товарах
		/// </summary>
		/// <param name="info"></param>
		public void CreatePurchaseComponentReport(ExcelInfo info)
		{
			CreateExcel(info);

			InsertCellInWorksheet(new ExcelCellParameters
			{
				ColumnName = "A",
				RowIndex = 1,
				Text = info.Title,
				StyleInfo = ExcelStyleInfoType.Title
			});

			MergeCells(new ExcelMergeParameters
			{
				CellFromName = "A1",
				CellToName = "E1"
			});

			uint rowIndex = 2;
			foreach (var bg in info.PurchaseComponent)
			{
				InsertCellInWorksheet(new ExcelCellParameters
				{
					ColumnName = "A",
					RowIndex = rowIndex,
					Text = bg.Id.ToString(),
					StyleInfo = ExcelStyleInfoType.Text
				});
				rowIndex++;

				foreach (var build in bg.Builds)
				{
					InsertCellInWorksheet(new ExcelCellParameters
					{
						ColumnName = "B",
						RowIndex = rowIndex,
						Text = build.Build,
						StyleInfo = ExcelStyleInfoType.TextWithBroder
					});
					InsertCellInWorksheet(new ExcelCellParameters
					{
						ColumnName = "C",
						RowIndex = rowIndex,
						Text = build.count.ToString(),
						StyleInfo = ExcelStyleInfoType.TextWithBroder
					});

					rowIndex++;
					foreach (var component in build.Item3)
					{
						InsertCellInWorksheet(new ExcelCellParameters
						{
							ColumnName = "D",
							RowIndex = rowIndex,
							Text = component.Component,
							StyleInfo = ExcelStyleInfoType.TextWithBroder
						});
						InsertCellInWorksheet(new ExcelCellParameters
						{
							ColumnName = "E",
							RowIndex = rowIndex,
							Text = component.count.ToString(),
							StyleInfo = ExcelStyleInfoType.TextWithBroder
						});

						rowIndex++;
					}
				}

			}

			SaveExcel(info);
		}

		/// <summary>
		/// Создание excel-файла
		/// </summary>
		/// <param name="info"></param>
		protected abstract void CreateExcel(ExcelInfo info);

		/// <summary>
		/// Добавляем новую ячейку в лист
		/// </summary>
		/// <param name="cellParameters"></param>
		protected abstract void InsertCellInWorksheet(ExcelCellParameters excelParams);

		/// <summary>
		/// Объединение ячеек
		/// </summary>
		/// <param name="mergeParameters"></param>
		protected abstract void MergeCells(ExcelMergeParameters excelParams);

		/// <summary>
		/// Сохранение файла
		/// </summary>
		/// <param name="info"></param>
		protected abstract void SaveExcel(ExcelInfo info);
	}
}