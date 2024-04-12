using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Pravadnichok.ViewModel;

namespace Pravadnichok.ViewModel
{
    public partial class ContextMenuViewModel 
    {
        public BindingCommand FileCreate {  get; set; }
        public BindingCommand DeleteFile {  get; set; }
        public BindingCommand Edit {  get; set; }
        public BindingCommand FolderCreate { get; set; }

        public ContextMenuViewModel()
        {
            FileCreate = new BindingCommand(_ => CreateFile());
            DeleteFile = new BindingCommand(_ => Delete());
            Edit = new BindingCommand(_ => Rename());
            FolderCreate = new BindingCommand(_ => FolderCreate());
        }


        public void CreateFile() 
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"; 

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;

                File.Create(filePath).Close();

                MessageBox.Show("Файл успешно создан: " + filePath, "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            void CreateFolder()
            {
                string folderName = Interaction.InputBox("Введите имя новой папки:", "Создать папку", "");

                if (!string.IsNullOrWhiteSpace(folderName))
                {
                    try
                    {
                        string currentDirectory = Directory.GetCurrentDirectory();

                        string newFolderPath = Path.Combine(currentDirectory, folderName);
                        Directory.CreateDirectory(newFolderPath);

                        MessageBox.Show($"Папка '{folderName}' успешно создана.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при создании папки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Имя папки не может быть пустым.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            void Rename(string filePath)
            {
                if (!File.Exists(filePath))
                {
                    MessageBox.Show("Файл не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);

                string newFileName = Interaction.InputBox($"Введите новое имя для файла '{fileNameWithoutExtension}':", "Переименовать файл", fileNameWithoutExtension);

                if (!string.IsNullOrWhiteSpace(newFileName))
                {
                    try
                    {
                        string fileDirectory = Path.GetDirectoryName(filePath);

                        string fileExtension = Path.GetExtension(filePath);

                        string newFilePath = Path.Combine(fileDirectory, newFileName + fileExtension);

                        File.Move(filePath, newFilePath);

                        MessageBox.Show($"Файл успешно переименован в '{newFileName + fileExtension}'.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при переименовании файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Имя файла не может быть пустым.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }


        }
        public void Delete()
        {
            
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Выберите файл или папку для удаления";
            openFileDialog.Multiselect = true; 
            openFileDialog.CheckFileExists = true; 
            openFileDialog.CheckPathExists = true; 
            openFileDialog.Filter = "Все файлы (*.*)|*.*"; 

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string[] selectedPaths = openFileDialog.FileNames;

                foreach (string path in selectedPaths)
                {
                    try
                    {
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }
                        else if (Directory.Exists(path))
                        {
                            Directory.Delete(path, true);
                        }

                        MessageBox.Show($"Файл или папка '{Path.GetFileName(path)}' успешно удалены.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении файла или папки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}

