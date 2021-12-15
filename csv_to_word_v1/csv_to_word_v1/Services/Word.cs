using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WordOffice = Microsoft.Office.Interop.Word;

namespace csv_to_word_v1.Services
{
    class Word
    {
        private FileInfo _fileInfo;

        public Word(string fileName)
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

        internal bool Process(Dictionary<string, string> items)
        {
            WordOffice.Application app = null;
            
            try
            {            
                app = new WordOffice.Application();
                Object file = _fileInfo.FullName;
                Object missing = Type.Missing;

                app.Documents.Open(file);
                foreach (var item in items)
                {
                    WordOffice.Find find = app.Selection.Find;
                    find.Text = item.Key;
                    find.Replacement.Text = item.Value;

                    Object wrap = WordOffice.WdFindWrap.wdFindContinue;
                    Object replace = WordOffice.WdReplace.wdReplaceAll;
                    find.Execute(
                        FindText: Type.Missing,
                        MatchCase: false,
                        MatchWholeWord: false,
                        MatchWildcards: false,
                        MatchSoundsLike: missing,
                        MatchAllWordForms: false,
                        Forward: true,
                        Wrap: wrap,
                        Format:false,
                        ReplaceWith:missing, 
                        Replace:replace
                    );
                }

                Object newFileName = Path.Combine(_fileInfo.DirectoryName, 
                    DateTime.Now.ToString("yyyyMMdd HHmmss") + _fileInfo.Name);

                app.ActiveDocument.SaveAs2(newFileName);
                app.ActiveDocument.Close();
                app.ActiveDocument.Close();                

                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                if (app != null)
                {
                    app.Quit();
                }                
            }
        }
    }
}
