using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace bankomat.Karty
{
    public class KartaJunior : Karta
    {

        
        public decimal LimitWyplaty { get; set; }
        public decimal LimitWplaty { get; set; }



        public void Witaj(KartaJunior kartaJunior, List<KartaJunior> Jbankowe)
        {
            Console.Clear();          
            bool continueOperation = true;
            do
            {
                Console.WriteLine($"Witamy! {ImieWlasciciela} {NazwiskoWlasciciela}");
                Console.Clear();
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
                            Wplac( kartaJunior,Jbankowe);
                            break;
                        case 2:
                            Wyplac(kartaJunior, Jbankowe);
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

        public void Wplac(KartaJunior kartaJunior, List<KartaJunior> Jbankowe)
        {
            try
            {
                Jbankowe.Remove(kartaJunior);
                Console.WriteLine("Podaj kwotę do wpłaty:");
                if (decimal.TryParse(Console.ReadLine(), out decimal kwota) && kwota > 0 && kwota < LimitWplaty)
                {
                    kartaJunior.Saldo += kwota;
                    Console.WriteLine($"Wpłacono {kwota} zł. Nowe saldo: {Saldo} zł.");
                    Jbankowe.Add(kartaJunior);
                    string newJson = JsonConvert.SerializeObject(Jbankowe, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(@"C:/bankomat/bazadanych/Juniorowe.json", newJson);

                }
                else
                {
                    Console.WriteLine("Nieprawidłowa kwota do wpłaty lub przekroczony limit.");
                }
            }
            catch (Exception ex)
            {
                Jbankowe.Add(kartaJunior);
                Console.WriteLine(ex.Message);
            }   
        }

        public void Wyplac(KartaJunior kartaJunior, List<KartaJunior> Jbankowe)
        {
            try
            {
                Jbankowe.Remove(kartaJunior);
                Console.WriteLine("Podaj kwotę do wypłaty:");
                if (decimal.TryParse(Console.ReadLine(), out decimal kwota) && kwota > 0 && kwota < LimitWyplaty)
                {
                    if (kwota <= kartaJunior.Saldo)
                    {
                        kartaJunior.Saldo -= kwota;
                        Console.WriteLine($"Wypłacono {kwota} zł. Nowe saldo: {Saldo} zł.");
                        Jbankowe.Add(kartaJunior);
                        string newJson = JsonConvert.SerializeObject(Jbankowe, Newtonsoft.Json.Formatting.Indented);
                        File.WriteAllText(@"C:/bankomat/bazadanych/Juniorowe.json", newJson);
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
                Jbankowe.Add(kartaJunior);
                Console.WriteLine(ex.Message);
            }


        }

    }
}
