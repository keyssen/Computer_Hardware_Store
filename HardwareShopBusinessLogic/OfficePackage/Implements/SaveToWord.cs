using HardwareShopBusinessLogic.OfficePackage.HelperEnums;
using HardwareShopBusinessLogic.OfficePackage.HelperModels;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace HardwareShopBusinessLogic.OfficePackage.Implements
{
    public class SaveToWord : AbstractSaveToWord
    {
        private WordprocessingDocument? _wordDocument;

        private Body? _docBody;

        /// <summary>
        /// Получение типа выравнивания
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static JustificationValues GetJustificationValues(WordJustificationType type)
        {
            return type switch
            {
                WordJustificationType.Both => JustificationValues.Both,
                WordJustificationType.Center => JustificationValues.Center,
                _ => JustificationValues.Left,
            };
        }

        /// <summary>
        /// Настройки страницы
        /// </summary>
        /// <returns></returns>
        private static SectionProperties CreateSectionProperties()
        {
            var properties = new SectionProperties();

            var pageSize = new PageSize
            {
                Orient = PageOrientationValues.Portrait
            };

            properties.AppendChild(pageSize);

            return properties;
        }

        /// <summary>
        /// Задание форматирования для абзаца
        /// </summary>
        /// <param name="paragraphProperties"></param>
        /// <returns></returns>
        private static ParagraphProperties? CreateParagraphProperties(WordTextProperties? paragraphProperties)
        {
            if (paragraphProperties == null)
            {
                return null;
            }

            var properties = new ParagraphProperties();

            properties.AppendChild(new Justification()
            {
                Val = GetJustificationValues(paragraphProperties.JustificationType)
            });

            properties.AppendChild(new SpacingBetweenLines
            {
                LineRule = LineSpacingRuleValues.Auto
            });

            properties.AppendChild(new Indentation());

            var paragraphMarkRunProperties = new ParagraphMarkRunProperties();
            if (!string.IsNullOrEmpty(paragraphProperties.Size))
            {
                paragraphMarkRunProperties.AppendChild(new FontSize { Val = paragraphProperties.Size });
            }
            properties.AppendChild(paragraphMarkRunProperties);

            return properties;
        }

        protected override void CreateWord(WordInfo info)
        {
            _wordDocument = WordprocessingDocument.Create(info.FileName, WordprocessingDocumentType.Document);
            MainDocumentPart mainPart = _wordDocument.AddMainDocumentPart();
            mainPart.Document = new Document();
            _docBody = mainPart.Document.AppendChild(new Body());
        }

        protected override void CreateTable(List<WordRow> data)
        {
            if (_docBody == null || data == null)
            {
                return;
            }
            Table table = new Table();
            var tableProp = new TableProperties();
            tableProp.AppendChild(new TableLayout { Type = TableLayoutValues.Fixed });
            tableProp.AppendChild(new TableBorders(
                    new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                    new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                    new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                    new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                    new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                    new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 }
                    ));
            tableProp.AppendChild(new TableWidth { Type = TableWidthUnitValues.Auto });
            table.AppendChild(tableProp);
            TableGrid tableGrid = new TableGrid();
            for (int j = 0; j < data[0].Rows.Count; ++j)
            {
                tableGrid.AppendChild(new GridColumn() { Width = "1600" });
            }
            table.AppendChild(tableGrid);
            for (int i = 0; i < data.Count; ++i)
            {
                TableRow docRow = new TableRow();
                for (int j = 0; j < data[i].Rows.Count; ++j)
                {
                    var docParagraph = new Paragraph(); 
                    var docRun = new Run();
                    var runProperties = new RunProperties();

                    docParagraph.AppendChild(CreateParagraphProperties(data[i].Rows[j].Item2));
                    
                    runProperties.AppendChild(new RunFonts() { Ascii = "Times New Roman", ComplexScript = "Times New Roman", HighAnsi = "Times New Roman" });
                    runProperties.AppendChild(new FontSize { Val = data[i].Rows[j].Item2.Size == null ? data[i].Rows[j].Item2.Size : "24" });
                    if (data[i].Rows[j].Item2.Bold) 
                        runProperties.AppendChild(new Bold());

                    docRun.AppendChild(runProperties);
                    docRun.AppendChild(new Text { Text = data[i].Rows[j].Item1.ToString(), Space = SpaceProcessingModeValues.Preserve });

                    docParagraph.AppendChild(docRun);
                    TableCell docCell = new TableCell();
                    docCell.AppendChild(docParagraph);
                    docRow.AppendChild(docCell);
                }
                table.AppendChild(docRow);
            }
            _docBody.AppendChild(table);
        }

        protected override void CreateParagraph(WordParagraph paragraph)
        {
            if (_docBody == null || paragraph == null)
            {
                return;
            }
            var docParagraph = new Paragraph();

            docParagraph.AppendChild(CreateParagraphProperties(paragraph.TextProperties));

            foreach (var run in paragraph.Texts)
            {
                var docRun = new Run();

                var properties = new RunProperties();
                properties.AppendChild(new FontSize { Val = run.Item2.Size });
                if (run.Item2.Bold)
                {
                    properties.AppendChild(new Bold());
                }
                docRun.AppendChild(properties);

                docRun.AppendChild(new Text { Text = run.Item1, Space = SpaceProcessingModeValues.Preserve });

                docParagraph.AppendChild(docRun);
            }

            _docBody.AppendChild(docParagraph);
        }

        protected override void SaveWord(WordInfo info)
        {
            if (_docBody == null || _wordDocument == null)
            {
                return;
            }
            _docBody.AppendChild(CreateSectionProperties());

            _wordDocument.MainDocumentPart!.Document.Save();

            _wordDocument.Close();
        }
    }
}