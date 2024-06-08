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

    private static void EditNote()
    {
        Console.WriteLine("Please enter the name of the note\n");

        string fileName = Console.ReadLine().ToLower();

        if(File.Exists(NoteDirectory + fileName))
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(NoteDirectory + fileName);
                Console.Write(doc.SelectSingleNode("//body").InnerText);
                string readInput = Console.ReadLine();

                if(readInput.ToLower() == "cancel")
                {
                    Main(null);
                }
                else
                {
                    string newText = doc.SelectSingleNode("//body").InnerText = readInput;
                    doc.Save(NoteDirectory + fileName);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Couldn't edit this note because of an error: " + ex.Message);
            }
        }
        else
        {
            Console.WriteLine("Note not found");
        }
    }

    private static void DeleteNote()
    {
        Console.WriteLine("Please enter the name of the note\n");

        string fileName = Console.ReadLine();

        if(File.Exists(NoteDirectory + fileName))
        {
            Console.WriteLine("Are you sure you want to delete this note? (Y/N)\n");
            string confirmation = Console.ReadLine().ToLower();

            if (confirmation == "y")
            {
                try
                {
                    File.Delete(NoteDirectory + fileName);
                    Console.WriteLine("Note deleted successfully");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("couldn't delete the file because of an error: " + ex.Message);
                }
            }
            else if(confirmation == "n")
            {
                Main(null);
            }
            else
            {
                Console.WriteLine("Invalid input");
                DeleteNote();
            }
        }
    }

    private static void ShowNotes()
    {
        string NoteLocation = NoteDirectory;

        DirectoryInfo Dir = new DirectoryInfo(NoteLocation);

        if (Directory.Exists(NoteLocation))
        {

            FileInfo[] NoteFiles = Dir.GetFiles("*.xml");

            if (NoteFiles.Count() != 0)
            {

                Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop + 2);
                Console.WriteLine("+------------+");
                foreach (var item in NoteFiles)
                {
                    Console.WriteLine("  " + item.Name);
                }

                Console.WriteLine(Environment.NewLine);
            }
            else
            {
                Console.WriteLine("No notes found.\n");
            }
        }
        else
        {

            Console.WriteLine(" Directory does not exist.....creating directory\n");

            Directory.CreateDirectory(NoteLocation);

            Console.WriteLine(" Directory: " + NoteLocation + " created successfully.\n");

        }
    }

    private static void NotesDirectory()
    {
        Process.Start("explorer.exe", NoteDirectory);
    }
}