﻿using ReadPhoto;
using Tesseract;

class Program
{
    static void Main(string[] args)
    {
        string imagePath = @"C:\Ulern\ReadPhoto\ReadPhoto\ReadPhoto\bin\Photo";

        string[] imageFiles = Directory.GetFiles(imagePath, "*.jpg");

        using (var engine = new TesseractEngine(@"tessdata", "por", EngineMode.Default))
        {
            using (var img = Pix.LoadFromFile(imagePath))
            {
                using (var page = engine.Process(img))
                {
                    string text = page.GetText().Replace('\n', ' ');

                    Cliente cliente = ExtractClienteData(text);

                    string csvFilePath = @"C:\Ulern\ReadPhoto\ReadPhoto\ReadPhoto\bin\client_data.csv";

                    WriteToCSV(csvFilePath, cliente);
                }
            }
        }
    }

    static Cliente ExtractClienteData(string text)
    {
        string name = ExtractName(text);
        string phone = ExtractPhone(text);
        string email = ExtractEmail(text);

        Cliente cliente = new Cliente
        {
            Name = name,
            Phone = phone,
            Email = email
        };

        return cliente;
    }

    private static void WriteToCSV(string filePath, Cliente cliente)
    {
        string csvLine = $"{cliente.Name},{cliente.Phone},{cliente.Email}";

        File.AppendAllLines(filePath, new List<string> { csvLine });
    }

    private static string ExtractName(string text)
    {
        string[] targetWords = { "Telefone", "Email" };

        int index = -1;
        foreach (string targetWord in targetWords)
        {
            index = text.IndexOf(targetWord);
            if (index != -1)
            {
                break;
            }
        }

        if (index == -1)
        {
            return text;
        }

        string filledString = text.Substring(0, index);
        return filledString;
    }

    private static string ExtractPhone(string text)
    {
        int index = -1;
        string[] lines = AddTextWithoutEmptyValue(text);
        foreach (var phone in lines)
        {
            index++;

            if (phone.StartsWith("Telefone", StringComparison.OrdinalIgnoreCase))
            {
                var telefone = lines[index + 1];
                return telefone;
            }
        }
        return null;
    }

    private static string ExtractEmail(string text)
    {
        int index = -1;
        string[] lines = AddTextWithoutEmptyValue(text);
        foreach (var email in lines)
        {
            index++;

            if (email.StartsWith("Email", StringComparison.OrdinalIgnoreCase))
            {
                var mail = lines[index + 1];
                return mail;
            }
        }
        return null;
    }

    private static string[] AddTextWithoutEmptyValue(string text)
    {
        return text.Split(' ').Where(x => !string.IsNullOrEmpty(x)).ToArray();
    }
}
