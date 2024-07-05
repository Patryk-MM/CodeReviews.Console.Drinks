﻿using jollejonas.Drinks.Services;
using jollejonas.Drinks.Models;
using Spectre.Console;

var drinksService = new DrinksService();
while (true)
{
    string selectedCategory = drinksService.SelectCategory().Result;
    string selectedDrink = drinksService.SelectDrink(selectedCategory).Result;
    Drink drink = drinksService.GetDrink(selectedDrink).Result;

    if (drink != null)
    {
        drinksService.DisplayDrinkDetails(drink);
        Console.WriteLine();
    }
    else
    {
        Console.WriteLine("Drink selection was canceled or failed.");
    }

    if (AnsiConsole.Confirm("Do you want to select another drink?"))
    {
        Console.Clear();
        continue;
    }
    else
    {
        break;
    }
}