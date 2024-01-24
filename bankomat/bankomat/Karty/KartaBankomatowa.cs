using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace bankomat.Karty
{
    public class KartaBankomatowa : Karta
    {
        public int LimitDzienny { get; set; }
        public int LimitLicznik { get; set; }


        public void Witaj(KartaBankomatowa kartaBankomatowa, List<KartaBankomatowa> Bbankomatowe)
        {
            Console.Clear();
            bool continueOperation = true;

            do
            {
                Console.Clear();
                Console.WriteLine($"Witamy! {ImieWlasciciela} {NazwiskoWlasciciela}");
                Console.WriteLine();
                Console.WriteLine("Wybierz operację: ");
                Console.WriteLine("1 ----- Wpłata na konto");
                Console.WriteLine("2 ----- Wypłata pieniędzy z konta ");
                Console.WriteLine("3 ----- Sprawdz Saldo");
              
                Console.WriteLine("4 ----- Zamknij aplikację");

                int wyborOperacji;

                if (int.TryParse(Console.ReadLine(), out wyborOperacji))
                {
                    switch (wyborOperacji)
                    {
                        case 1:
                            Wplac(kartaBankomatowa, Bbankomatowe);
                            break;
                        case 2:
                            Wyplac(kartaBankomatowa, Bbankomatowe);
                            break;
                        case 3:
                            Console.WriteLine($"Twoje obecne saldo wynosi: {Saldo}");
                            break;
                        case 4:
                            continueOperation = false;
                            break;
                        default:
                            Console.WriteLine("Nieoczekiwany błąd");
                            break;
                    }

                    if (continueOperation)
                    {
                        Console.WriteLine("Czy chcesz kontynuować? (T/N)");
                        char decision = Console.ReadKey().KeyChar;
                        continueOperation = (decision == 'T' || decision == 't');
                    }
                }
            } while (continueOperation);
        }

        public void Wplac(KartaBankomatowa kartaBankomatowa, List<KartaBankomatowa> Bbankomatowe)
        {
            try
            {
                Bbankomatowe.Remove(kartaBankomatowa);
                Console.WriteLine("Podaj kwotę do wpłaty:");
                if (decimal.TryParse(Console.ReadLine(), out decimal kwota) && kwota > 0)
                {
                    kartaBankomatowa.Saldo += kwota;
                    Console.WriteLine($"Wpłacono {kwota} zł. Nowe saldo: {Saldo} zł.");
                    Bbankomatowe.Add(kartaBankomatowa);
                    string newJson = JsonConvert.SerializeObject(Bbankomatowe, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(@"C:/bankomat/bazadanych/Bankomatowe.json", newJson);

                }
                else
                {
                    Console.WriteLine("Nieprawidłowa kwota do wpłaty lub przekroczony limit.");
                }
            }
            catch(Exception ex)
            {
                Bbankomatowe.Add(kartaBankomatowa);
                Console.WriteLine(ex.Message);
            }
        }

        public void Wyplac(KartaBankomatowa kartaBankomatowa, List<KartaBankomatowa> Bbankomatowe)
        {
            Console.WriteLine("Podaj kwotę do wypłaty:");
            
            try
            {
                Bbankomatowe.Remove(kartaBankomatowa);
                if (decimal.TryParse(Console.ReadLine(), out decimal kwota) && kwota > 0)
                {
                    if (kwota <= kartaBankomatowa.Saldo && kartaBankomatowa.LimitLicznik < kartaBankomatowa.LimitDzienny)
                    {
                        kartaBankomatowa.Saldo -= kwota;
                        kartaBankomatowa.LimitLicznik++;
                        Console.WriteLine($"Wypłacono {kwota} zł. Nowe saldo: {Saldo} zł.");
                        Bbankomatowe.Add(kartaBankomatowa);
                        string newJson = JsonConvert.SerializeObject(Bbankomatowe, Newtonsoft.Json.Formatting.Indented);
                        File.WriteAllText(@"C:/bankomat/bazadanych/Bankomatowe.json", newJson);
                    }
                    else
                    {
                        Console.WriteLine("Nieprawidłowa kwota do wypłaty lub przekroczony limit.");
                    }
                }
                else
                {
                    Console.WriteLine("Nieprawidłowa kwota do wypłaty.");
                }
            }
            catch (Exception ex)
            {
                Bbankomatowe.Add(kartaBankomatowa);
                Console.WriteLine(ex.Message);
            }            
        }

    }

}
