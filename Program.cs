namespace NotesAppCommandLine;
using System.Xml;
using System.Xml.Linq;
using System.Diagnostics;

internal class Program
{
    static void Main(string[] args)
    {
        ReadCommand();
        Console.ReadLine();
    }
    private static string NoteDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Notes\";

    private static void ReadCommand()
    {
        Console.Write(Directory.GetDirectoryRoot(NoteDirectory));
        string Command = Console.ReadLine();

        switch (Command.ToLower())
        {

            case "new":
                NewNote();
                Main(null);
                break;
            case "edit":
                EditNote();
                Main(null);
                break;
            case "read":
                ReadNote();
                Main(null);
                break;
            case "delete":
                DeleteNote();
                Main(null);
                break;
            case "shownotes":
                ShowNotes();
                Main(null);
                break;
            case "dir":
                NotesDirectory();
                Main(null);
                break;
            case "cls":
                Console.Clear();
                Main(null);
                break;
            case "exit":
                Exit();
                break;
            default:
                CommandsAvailable();
                Main(null);
                break;
        }
    }

    private static void NewNote()
    {
        Console.WriteLine("Please enter your note:\n");
        string input = Console.ReadLine();

        XmlWriterSettings writerSettings = new XmlWriterSettings();

        writerSettings.CheckCharacters = false;
        writerSettings.ConformanceLevel = ConformanceLevel.Auto;
        writerSettings.Indent = true;

        string fileName = DateTime.Now.ToString("dd-MM-yy") + ".xml";

        using (XmlWriter NewNote = XmlWriter.Create(NoteDirectory + fileName, writerSettings))
        {
            NewNote.WriteStartDocument();
            NewNote.WriteStartElement("Note");
            NewNote.WriteElementString("body", input);
            NewNote.WriteEndElement();

            NewNote.Flush();
            NewNote.Close();
        }
    }
}