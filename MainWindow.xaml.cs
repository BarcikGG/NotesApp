using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using static NotesApp.MainWindow;
using Xamarin.Forms.PlatformConfiguration;

namespace NotesApp
{
    public partial class MainWindow : Window
    {
        private string NotesFileName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\notes.json";
        //private const string NotesFileName = "C:\\Users\\danil\\Documents\\notes.json";
        static public List<Note> Notes = new List<Note>();

        public MainWindow()
        {
            InitializeComponent();
            datePicker.SelectedDate = DateTime.Today.Date;
            Notes.Clear();
            if (File.Exists(NotesFileName))
            {
                Notes = JsonSerialization.Deserialize<List<Note>>(NotesFileName);
            }
            else File.Create(NotesFileName);
        }

        public class Note
        {
            public string Title { get; set; }
            public string Text { get; set; }
            public DateTime Date { get; set; }
            public DateTime ToDate { get; set; }
        }

        private void addNoteButton_Click(object sender, RoutedEventArgs e)
        {
            if (datePicker.SelectedDate == null) MessageBox.Show("Вы не выбрали дату");
            else
            {
                AddNotes window = new AddNotes();
                Note note = new Note();
                note.Date = Convert.ToDateTime(datePicker.SelectedDate);
                if (window.ShowDialog() == true)
                {
                    note.Title = window.titleTextBox.Text;
                    note.Text = window.textTextBox.Text;
                    note.ToDate = Convert.ToDateTime(window.TodoPicker.SelectedDate).Date;
                    if (String.IsNullOrEmpty(note.Title))
                    {
                        MessageBox.Show("Заголовок не может быть пустым");
                    }
                    else
                    {
                        Notes.Add(note);
                        notesList.Items.Add(note);
                        notesList.Items.Refresh();
                    }
                }
                saveNotesButton.IsEnabled = true;
            }
        }

        private void editNoteButton_Click_1(object sender, RoutedEventArgs e)
        {
            Note selectedNote = notesList.SelectedItem as Note;
            if (selectedNote == null)
            {
                MessageBox.Show("Заметка не выбрана");
            }
            else
            {
                EditNoteWindow editNoteWindow = new EditNoteWindow(selectedNote);
                if (editNoteWindow.ShowDialog() == true)
                {
                    selectedNote.Text = editNoteWindow.textTextBox.Text;
                    selectedNote.Title = editNoteWindow.titleTextBox.Text;
                    selectedNote.ToDate = Convert.ToDateTime(editNoteWindow.TodoPicker.SelectedDate).Date;
                    notesList.Items.Refresh();
                }
                saveNotesButton.IsEnabled = true;
            }
        }

        private void deleteNoteButton_Click(object sender, RoutedEventArgs e)
        {
            Note selectedNote = notesList.SelectedItem as Note;
            if (selectedNote == null)
            {
                MessageBox.Show("Заметка не выбрана");
            }

            var result = MessageBox.Show("Точно хотите удалить заметку?", "Удаление заметки", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                notesList.Items.Remove(selectedNote);
            }
            saveNotesButton.IsEnabled = true;
        }

        private void saveNotesButton_Click(object sender, RoutedEventArgs e)
        {
            string NotesFileName = "C:\\Users\\danil\\Documents\\notes.json";
            List<Note> notes_save = new List<Note>();
            foreach (Note note in Notes)
            {
                notes_save.Add(note);
            }
            JsonSerialization.Serialize(notes_save, NotesFileName);
            saveNotesButton.IsEnabled = false;
        }

        private void datePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            notesList.Items.Clear();

            foreach (Note note in Notes)
            {
                if (note.Date.DayOfYear == Convert.ToDateTime(datePicker.SelectedDate).DayOfYear)
                {
                    notesList.Items.Add(note);
                }
            }
            notesList.Items.Refresh();
        }

        private void notesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Note selectedNote = notesList.SelectedItem as Note;
            if (selectedNote != null)
            {
                MessageBox.Show("Заголовок: " + selectedNote.Title 
                    + "\nТекст: " + selectedNote.Text+"\nВыполнить до: "+selectedNote.ToDate);
            }
        }
    }

    public class JsonSerialization
    {
        public static void Serialize<T>(T text, string fileName)
        {
            var json = JsonConvert.SerializeObject(text, Formatting.Indented);
            File.WriteAllText(fileName, json);
        }

        public static T Deserialize<T>(string fileName)
        {
            var json = File.ReadAllText(fileName);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }

}
