﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using SwedbankSharp;

namespace Swegbank
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.Write("Personal ID number: ");
            long idnumber = Convert.ToInt64(Console.ReadLine());
            Console.Write("Password: ");
            string password = Console.ReadLine();

            Swedbank client = new Swedbank(idnumber, password, "swedbank");

            Console.WriteLine("\nRetrieving list of accounts...");
            SwedbankSharp.JsonSchemas.Overview accounts = client.AccountList();

            List<SwedbankSharp.JsonSchemas.BankAccount> bankAccounts = new List<SwedbankSharp.JsonSchemas.BankAccount>();
            bankAccounts.AddRange(accounts.transactionAccounts);
            bankAccounts.AddRange(accounts.savingAccounts);
            bankAccounts.AddRange(accounts.cardAccounts);

            int i = 1;
            foreach (SwedbankSharp.JsonSchemas.BankAccount ta in bankAccounts)
            {
                Console.WriteLine(i + ". " + ta.name);
                i++;
            }

            Console.Write("Choose account: ");
            int no = ReadKey();

            Console.WriteLine("\nRetrieving account details...");
            SwedbankSharp.JsonSchemas.Transactions t = client.AccountDetails(bankAccounts[no-1].id);

            Console.WriteLine("Account Name: " + t.account.name);
            Console.WriteLine("Account Balance: " + t.account.balance + t.account.currency);

            client.Terminate();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        static int ReadKey()
        {
            while (true)
            {
                ConsoleKeyInfo choice = Console.ReadKey();
                Console.WriteLine();
                if (char.IsDigit(choice.KeyChar))
                {
                    int answer = Convert.ToInt32(choice.KeyChar);
                    return answer - 48;
                }
                Console.WriteLine("\nSorry, you need to input a number.");
            }
        }

    }
}