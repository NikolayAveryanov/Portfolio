using System;
using Word = Microsoft.Office.Interop.Word;
using System.IO;

public class Document
{
	public Document()
	{
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
	}
}
