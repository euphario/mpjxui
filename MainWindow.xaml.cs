using System;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using net.sf.mpxj;
using net.sf.mpxj.reader;
using net.sf.mpxj.mspdi;

namespace MPXJUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private string fileName = "Please drop your .xer file";
        public string File
        {
            get
            {
                return fileName;
            }

            set
            {
                if (fileName != value)
                {
                    fileName = value;
                    OnPropertyChanged();
                }
            }
        }

        private void ImagePanel_Drop(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                // Assuming you have one file that you care about, pass it off to whatever
                // handling code you have defined.
                File = files[0];
                HandleFileOpen(files[0]);
            }
        }

        private int HandleFileOpen(string file)
        {
            try
            {
                ProjectReader reader = ProjectReaderUtility.getProjectReader(file);
                ProjectFile projectFile = reader.read(file);

      //
      // Create TEXT10 _ Activity_ID
      //
      CustomFieldContainer customFields = file.getCustomFields();
      CustomField field = customFields.getCustomField(TaskField.TEXT10);
      field.setAlias("Activity_ID");
    //  task.setText(10, task.getActivityID())



                MSPDIWriter writer = new MSPDIWriter();
                writer.write(projectFile, file + ".xml");
                File = "Done";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 1;
            }

            return 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
        }
    }
}
