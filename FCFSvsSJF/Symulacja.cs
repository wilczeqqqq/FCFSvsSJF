using FCFSvsSJF;
using System;

/*
 * nazwa: FCFSvsSJF - porównanie działania algorytmów na 100 zbiorach po 100 procesów
 * autor: Filip Dowhan 259683
 * wersja: 1.0 - 08.01.2022
 */

class Symulacja
{
    List<Proces> listaProcesow = new();

    /* FUNKCJA DO WYŚWIETLANIA TABELKI Z KOLEJNYMI PROCESAMI WRAZ ZE WSZYSTKIMI CZASAMI (unused,devtool)
     *
    private void wyswietlProcesy()
    {
        Console.WriteLine("ID\tAT\tBT\tFT\tTAT\tWT");
        foreach (Proces proces in listaProcesow)
        {
            Console.WriteLine(proces.ID + "\t" + proces.AT + "\t" + proces.BT + "\t" + proces.FT + "\t" + proces.TAT + "\t" + proces.WT);
        }
    }
     */

    /* FUNKCJA SŁUŻĄCA DO WYGENEROWANIA PLIKÓW Z DANYMI PROCESÓW (unused,devtool)
     *
    private static async Task generujPlikiDanych()
    {
        Random random = new Random();

        for (int i = 1; i < 101; i++)
        {
            using StreamWriter file = new(i+".txt", append: true);
            for (int j = 0; j < 100; j++)
            {
                await file.WriteLineAsync((random.Next(0,100))+"\t"+(random.Next(1,20)));
            }

        }
    }
     */

    private void dodajProcesy(int x)
    {
        listaProcesow.Clear();

        string nazwaPliku = "DANE\\"+x+".txt";
        int id = -1;
        string[] plik = File.ReadAllLines(nazwaPliku);

        foreach (string linia in plik)
        {
            string[] bits = linia.Split('\t');
            int at = int.Parse(bits[0]);
            int bt = int.Parse(bits[1]);
            id++;
            listaProcesow.Add(new Proces(id, at, bt));
        }
    }

    private void statystyki(int x, bool z)
    {
        double tat_sum = 0, wt_sum = 0;

        foreach (Proces proces in listaProcesow)
        {
            tat_sum += proces.TAT;
            wt_sum += proces.WT;
        }

        if (z)
        {
            Console.WriteLine("[FCFS " + x + "] Avg. Turnaround Time = " + tat_sum / listaProcesow.Count() + "\t" + "Avg. Waiting Time = " + wt_sum / listaProcesow.Count());
        }
        else
        {
            Console.WriteLine("[SJF " + x + "] Avg. Turnaround Time = " + tat_sum / listaProcesow.Count() + "\t" + "Avg. Waiting Time = " + wt_sum / listaProcesow.Count());
        }

    }

    private void FCFS(int x, bool z)
    {
        listaProcesow = listaProcesow.OrderBy(proces => proces.AT).ToList();
        
        listaProcesow[0].WT = 0;
        int temp;

        for (int i = 1; i < listaProcesow.Count; i++)
        {
            temp = listaProcesow[i - 1].BT + listaProcesow[i - 1].WT + listaProcesow[i - 1].AT - listaProcesow[i].AT;
            if (temp < 0)
            {
                listaProcesow[i].WT = 0;
            }
            else
            {
                listaProcesow[i].WT = temp;
            }

        }

        foreach (Proces proces in listaProcesow)
        {
            proces.TAT = proces.BT + proces.WT;
            proces.FT = proces.BT + proces.WT + proces.AT;
        }

        //wyswietlProcesy();
        statystyki(x, z);
    }

    private void SJF(int x, bool z)
    {
        listaProcesow = listaProcesow.OrderBy(proces => proces.AT).ToList();

        int temp, low, val = -1;
        Proces temp2;

        listaProcesow[0].FT = listaProcesow[0].AT + listaProcesow[0].BT;
        listaProcesow[0].TAT = listaProcesow[0].BT;
        listaProcesow[0].WT = 0;

        for (int i = 1; i < listaProcesow.Count; i++)
        {
            temp = listaProcesow[i - 1].FT;
            low = listaProcesow[i].BT;
            for (int j = i; j < listaProcesow.Count; j++)
            {
                if (temp >= listaProcesow[j].AT && low >= listaProcesow[j].BT)
                {
                    low = listaProcesow[j].BT;
                    val = j;
                }
            }

            if (val == -1)
            {
                continue;
            }

            listaProcesow[val].FT = temp + listaProcesow[val].BT;
            listaProcesow[val].TAT = listaProcesow[val].FT - listaProcesow[val].AT;

            temp = listaProcesow[val].TAT - listaProcesow[val].BT;
            if (temp < 0)
            {
                listaProcesow[val].WT = 0;
            }
            else
            {
                listaProcesow[val].WT = temp;
            }
            
            temp2 = listaProcesow[val];
            listaProcesow[val] = listaProcesow[i];
            listaProcesow[i] = temp2;
        }
        //wyswietlProcesy();
        statystyki(x, z);
    }

    public void startSymulacji(int x)
    {
        dodajProcesy(x);
        FCFS(x, true);
        SJF(x, false);
        Console.WriteLine("\n");
    }

    // Main
    static void Main(string[] args)
    {
        Symulacja symulacja = new Symulacja();
        for (int i = 1; i < 101; i++)
        {
            symulacja.startSymulacji(i);
        }
    }
}