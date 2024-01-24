using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using bankomat;
using bankomat.Karty;
using Newtonsoft.Json;
class Program
{
    static void Main(string[] args)
    {
        OdczytPlikow odczytPlikow = new OdczytPlikow();
        odczytPlikow.Odczyt();
        Console.WriteLine("=====================================================================================");
        Console.WriteLine("=================================\"Bankomat:\"=======================================");
        Console.WriteLine("=====================================================================================");

        Console.WriteLine();
        Console.WriteLine("Uwaga!");
        Console.WriteLine("Bankomat obsluguje karty: ");
        Console.WriteLine("Visa", "Mastercard", "American Express", "Visa Electron");
        Console.WriteLine("Prosze podac PIN");

        string pin = "";
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true);
            if (char.IsDigit(key.KeyChar) && pin.Length < 4)
            {
                pin += key.KeyChar;
                Console.Write("*");
            }
            // Obsłuż klawisz Backspace
            else if (key.Key == ConsoleKey.Backspace && pin.Length > 0)
            {
                pin = pin.Substring(0, pin.Length - 1);
                Console.Write("\b \b"); 
            }
        } while (key.Key != ConsoleKey.Enter || pin.Length != 4);

        var kartaBankomatowa = odczytPlikow.Bbankomatowe.Where
            (x => x.Pin.ToString() == pin).FirstOrDefault();
        var kartaJunior = odczytPlikow.Jbankomatowe.Where
            (x => x.Pin.ToString() == pin).FirstOrDefault();
        var kartaKredytowa = odczytPlikow.Kbankomatowe.Where
            (x => x.Pin.ToString() == pin).FirstOrDefault();
        var kartaPlatnicza = odczytPlikow.Pbankomatowe.Where
            (x => x.Pin.ToString() == pin).FirstOrDefault();

        Console.WriteLine();
        Console.WriteLine();

        if (kartaBankomatowa != null)
        {
            if(kartaBankomatowa.NazwaKarty != "Visa" ||
                kartaBankomatowa.NazwaKarty != "MasterCard" ||
                kartaBankomatowa.NazwaKarty != "American Express" ||
                kartaBankomatowa.NazwaKarty != "Visa Electron")
            {
                Console.WriteLine("Ta karta nie moze byc obsluzona! ");
            }

            Console.WriteLine("znaleziono karte");
            Console.WriteLine("karta bankomatowa");
            kartaBankomatowa.Witaj(kartaBankomatowa, odczytPlikow.Bbankomatowe);

        }

        

        else if(kartaJunior != null)
        {
            if (kartaJunior.NazwaKarty !=
                "Visa" && kartaJunior.NazwaKarty != 
                "MasterCard" && kartaJunior.NazwaKarty !=
                "American Express" && kartaJunior.NazwaKarty !=
                "Visa Electron")
            {
                Console.WriteLine("Ta karta nie moze byc obsluzona ");
            }
            else
            {
                kartaJunior.Witaj(kartaJunior, odczytPlikow.Jbankomatowe);
            } 
        }
        

        else if (kartaKredytowa != null)
        {
            if (kartaKredytowa.NazwaKarty != "Visa" || kartaKredytowa.NazwaKarty != "MasterCard" || kartaKredytowa.NazwaKarty != "American Express" || kartaKredytowa.NazwaKarty != "Visa Electron")
            {
                Console.WriteLine("Ta karta nie moze byc obsluzona ");
            }
            Console.WriteLine("znaleziono karte");
            Console.WriteLine("kredytowa");
            kartaKredytowa.Witaj(kartaKredytowa, odczytPlikow.Kbankomatowe);
        }
        

        else if (kartaPlatnicza != null)
        {
            if (kartaPlatnicza.NazwaKarty != "Visa" || kartaPlatnicza.NazwaKarty != "MasterCard" || kartaPlatnicza.NazwaKarty != "American Express" || kartaPlatnicza.NazwaKarty != "Visa Electron")
            {
                Console.WriteLine("Ta karta nie moze byc obsluzona ");
            }
            Console.WriteLine("znaleziono karte ");
            Console.WriteLine("platnicza");
            kartaPlatnicza.Witaj(kartaPlatnicza, odczytPlikow.Pbankomatowe);
        }
        else
        {
            Console.WriteLine("Błędny kod pin");
         
        }
        
    }

}