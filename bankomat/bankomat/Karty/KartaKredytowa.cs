using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace bankomat.Karty
{
    public class KartaKredytowa : Karta
    {
        
        public decimal LimitKredytowy { get; set; }
        public void Witaj(KartaKredytowa kartaKredytowa, List<KartaKredytowa> Kbankomatowe)
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
                            Wplac(kartaKredytowa, Kbankomatowe);
                            break;
                        case 2:
                            Wyplac(kartaKredytowa, Kbankomatowe);
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

        public void Wplac(KartaKredytowa kartaKredytowa, List<KartaKredytowa> Kbankomatowe)
        {
            try
            {
                Kbankomatowe.Remove(kartaKredytowa);
                Console.WriteLine("Podaj kwotę do wpłaty:");
                if (decimal.TryParse(Console.ReadLine(), out decimal kwota) && kwota > 0)
                {
                    kartaKredytowa.Saldo += kwota;
                    Console.WriteLine($"Wpłacono {kwota} zł. Nowe saldo: {kartaKredytowa.Saldo} zł.");
                    Kbankomatowe.Add(kartaKredytowa);
                    string newJson = JsonConvert.SerializeObject(Kbankomatowe, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(@"C:/bankomat/bazadanych/Kredytowe.json", newJson);
                }
                else
                {
                    Console.WriteLine("Nieprawidłowa kwota do wpłaty lub przekroczony limit.");
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }          
        }
        public void Wyplac(KartaKredytowa kartaKredytowa, List<KartaKredytowa> Kbankomatowe)
        {
            try
            {
                Kbankomatowe.Remove(kartaKredytowa);
                Console.WriteLine("podaj kwote do wyplaty");
                int kwota = int.Parse(Console.ReadLine());
                if (kwota > 0 && kwota < kartaKredytowa.LimitKredytowy)
                {
                    Saldo -= (int)kwota;
                    Console.WriteLine($"Wypłacono {kwota} zł. Nowe saldo: {kartaKredytowa.Saldo} zł.");
                    Kbankomatowe.Add(kartaKredytowa);
                    string newJson = JsonConvert.SerializeObject(Kbankomatowe, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(@"C:/bankomat/bazadanych/Kredytowe.json", newJson);
                }
                else
                {
                    Console.WriteLine("Nieprawidłowa kwota do wypłaty lub przekroczony limit.");
                }
            }

            catch(Exception ex)
            {
                Kbankomatowe.Add(kartaKredytowa);
                Console.WriteLine(ex.Message);
            }
        }

    }
}
