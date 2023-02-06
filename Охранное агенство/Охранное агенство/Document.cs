using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Word = Microsoft.Office.Interop.Word;
using System.Windows.Forms;

namespace Охранное_агенство
{
    internal class Document
    {
        FileInfo _fileInfo;
        public Document(string fileName)
        {
            if (File.Exists(fileName))
            {
               _fileInfo = new FileInfo(fileName);
            }
            else
            {
                throw new ArgumentException("File not found");
            }
        }

        /// <summary>
        /// Создание тектового документ
        /// </summary>
        /// <param name="items"></param>
        /// <param name="type">Тип документа (договор, отчёт и тд)</param>
        /// <returns></returns>
        internal bool Process(Dictionary<string, string> items, string type)
        {
            Word.Application app = null;
            try
            {
                app = new Word.Application();
                Object file = _fileInfo.FullName;

                Object missing = Type.Missing;

                app.Documents.Open(file);
                foreach (var item in items)
                {
                    Word.Find find = app.Selection.Find;
                    find.Text = item.Key;
                    find.Replacement.Text = item.Value;

                    Object wrap = Word.WdFindWrap.wdFindContinue;
                    Object replace = Word.WdReplace.wdReplaceAll;

                    find.Execute(FindText: Type.Missing, MatchCase: false, MatchWholeWord: false, MatchWildcards: false, MatchSoundsLike: missing, MatchAllWordForms: false, Forward: true, Wrap: wrap, Format: false, ReplaceWith: missing, Replace: replace);
                }
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DateTime.Now.ToString("yyyyMMdd HHmmss") + " "+type);
                Object newFileName = path;
                app.ActiveDocument.SaveAs2(newFileName);
                app.ActiveDocument.Close();
                //открыть созданный отчет программой по умолчанию
                System.Diagnostics.Process.Start(path + ".docx");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (app != null)
                {
                    app.Quit();
                }
            }
            return false;
        }
    }
}
