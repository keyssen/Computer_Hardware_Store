using DocumentFormat.OpenXml.EMMA;
using HardwareShopBusinessLogic.OfficePackage.HelperEnums;
using HardwareShopBusinessLogic.OfficePackage.HelperModels;
using MigraDoc.Rendering;

namespace HardwareShopBusinessLogic.OfficePackage
{
    public abstract class AbstractSaveToPdf
    {
        public void GetPurchaseReportFile(PdfInfo info)
        {
            CreatePdf(info);

            CreateParagraph(new PdfParagraph
            {
                Text = info.Title,
                Style = "NormalTitle",
                ParagraphAlignment = PdfParagraphAlignmentType.Center
            });

            CreateParagraph(new PdfParagraph
            {
                Text = $"за период с {info.DateFrom.ToShortDateString()} " +
                    $"по {info.DateTo.ToShortDateString()}",
                Style = "Normal",
                ParagraphAlignment = PdfParagraphAlignmentType.Center
            });

            CreateTable(new List<string> { "3cm", "4cm", "3cm", "4cm", "4cm" });

            CreateRow(new PdfRowParameters
            {
                Texts = new List<string> { "Покупка", "Дата покупки", "Цена", "Комментарии", "Комплектующие" },
                Style = "NormalTitle",
                ParagraphAlignment = PdfParagraphAlignmentType.Center
            });

            foreach (var record in info.ReportPurchases)
            {
                List<string> comments = record.Comments;
                List<string> components = record.Components;
                int recordHeight = Math.Max(comments.Count + 1, components.Count + 1);
                for (int i = 0; i < recordHeight; i++)
                {
                    List<string> cellsData = new() { "", "", "", "", "" };
                    if (i == 0)
                    {
                        cellsData[0] = record.Id.ToString();
                        cellsData[1] = record.PurchaseDate.ToShortDateString();
                        cellsData[2] = record.PurchaseSum.ToString("0.00") + " р.";
                        CreateRow(new PdfRowParameters
                        {
                            Texts = cellsData,
                            Style = "Normal",
                            ParagraphAlignment = PdfParagraphAlignmentType.Left
                        });
                        continue;
                    }
                    int k = i - 1;
                    if (k < comments.Count)
                    {
                        cellsData[3] = comments[k];
                    }
                    if (k < components.Count)
                    {
                        cellsData[4] = components[k];
                    }
                    CreateRow(new PdfRowParameters
                    {
                        Texts = cellsData,
                        Style = "Normal",
                        ParagraphAlignment = PdfParagraphAlignmentType.Left
                    });
                }
            }
            CreateParagraph(new PdfParagraph { Text = $"Итого: {info.ReportPurchases.Sum(x => x.PurchaseSum)}\t", Style = "Normal", ParagraphAlignment = PdfParagraphAlignmentType.Left });
            SavePdf(info);
        }

        public void CreateComponentsReport(PdfInfo info)
        {
            CreatePdf(info);
            CreateParagraph(new PdfParagraph { Text = info.Title, Style = "NormalTitle", ParagraphAlignment = PdfParagraphAlignmentType.Center });
            CreateParagraph(new PdfParagraph { Text = $"с {info.DateFrom.ToShortDateString()} по {info.DateTo.ToShortDateString()}", Style = "Normal", ParagraphAlignment = PdfParagraphAlignmentType.Center });

            CreateTable(new List<string> { "5cm", "5cm", "3cm" });

            CreateRow(new PdfRowParameters
            {
                Texts = new List<string> { "Комплектующее", "Товар/Сборка", "Количество" },
                Style = "NormalTitle",
                ParagraphAlignment = PdfParagraphAlignmentType.Center
            });

            foreach (var record in info.ReportComponents)
            {
                CreateRow(new PdfRowParameters
                {
                    Texts = new List<string> { record.ComponentName, "", "" },
                    Style = "Normal",
                    ParagraphAlignment = PdfParagraphAlignmentType.Left
                });
                foreach (var goodOrBuild in record.GoodOrBuilds)
                {
                    CreateRow(new PdfRowParameters
                    {
                        Texts = new List<string> { "", goodOrBuild.Item1, goodOrBuild.Item2.ToString() },
                        Style = "Normal",
                        ParagraphAlignment = PdfParagraphAlignmentType.Left
                    });
                }
                CreateRow(new PdfRowParameters
                {
                    Texts = new List<string> { "Итого", "", record.TotalCount.ToString() },
                    Style = "Normal",
                    ParagraphAlignment = PdfParagraphAlignmentType.Left
                });
            }

            SavePdf(info);
        }

        /// <summary>
		/// Создание doc-файла
		/// </summary>
		/// <param name="info"></param>
        protected abstract void CreatePdf(PdfInfo info);

        /// <summary>
        /// Создание параграфа с текстом
        /// </summary>
        /// <param name="title"></param>
        /// <param name="style"></param>
        protected abstract void CreateParagraph(PdfParagraph paragraph);

        /// <summary>
        /// Создание таблицы
        /// </summary>
        /// <param name="title"></param>
        /// <param name="style"></param>
        protected abstract void CreateTable(List<string> columns);

        /// <summary>
        /// Создание и заполнение строки
        /// </summary>
        /// <param name="rowParameters"></param>
        protected abstract void CreateRow(PdfRowParameters rowParameters);

        /// <summary>
		/// Сохранение файла
		/// </summary>
		/// <param name="info"></param>
        protected abstract void SavePdf(PdfInfo info);
    }
}
