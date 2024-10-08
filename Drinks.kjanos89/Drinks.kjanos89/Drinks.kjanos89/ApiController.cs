﻿using Drinks.kjanos89.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Reflection;
using System.Web;

namespace Drinks.kjanos89;

public class ApiController
{
    public List<string> categoryNames = new List<string>();
    public List<string> drinkIds = new List<string>();

    public void GetCategories()
    {
        var client = new RestClient("http://www.thecocktaildb.com/api/json/v1/1/");
        var request = new RestRequest("list.php?c=list");
        var response = client.ExecuteAsync(request);
        if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
        {
            string rawResponse = response.Result.Content;
            var serialize = JsonConvert.DeserializeObject<Categories>(rawResponse);
            List<Category> categories = serialize.CategoryList;
            categoryNames = categories.Select(c => c.CategoryName).ToList();
            Menu.ShowData(categories, "Categories");
        }
        else
        {
            Console.WriteLine("Something went wrong.");
        }
    }

    public void GetDrinks(string choice)
    {
        var client = new RestClient("http://www.thecocktaildb.com/api/json/v1/1/");
        var request = new RestRequest($"filter.php?c={HttpUtility.UrlEncode(choice)}");
        var response = client.ExecuteAsync(request);
        if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
        {
            string rawResponse = response.Result.Content;
            var serialize = JsonConvert.DeserializeObject<DrinksClass>(rawResponse);
            List<Drink> drinks = serialize.DrinkList;
            drinkIds = drinks.Select(d => d.IdDrink).ToList();
            Menu.ShowData(drinks, "Drinks");
        }
        else
        {
            Console.WriteLine("Something went wrong.");
        }
    }

    public void GetIngredients(string drink)
    {
        Console.Clear();
        var client = new RestClient("http://www.thecocktaildb.com/api/json/v1/1/");
        var request = new RestRequest($"lookup.php?i={drink}");
        var response = client.ExecuteAsync(request);

        if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
        {
            string rawResponse = response.Result.Content;
            var serialize = JsonConvert.DeserializeObject<DrinkDetailObject>(rawResponse);
            List<DrinkDetail> returnedList = serialize.DrinkDetailList;
            DrinkDetail drinkDetail = returnedList[0];

            List<object> prepList = new();
            string formattedName = "";

            foreach (PropertyInfo prop in drinkDetail.GetType().GetProperties())
            {
                if (prop.Name.Contains("str"))
                {
                    formattedName = prop.Name.Substring(3);
                }

                if (!string.IsNullOrEmpty(prop.GetValue(drinkDetail)?.ToString()))
                {
                    prepList.Add(new
                    {
                        Key = formattedName,
                        Value = prop.GetValue(drinkDetail)
                    });
                }
            }

            Menu.ShowData(prepList, drinkDetail.StrDrink);
        }
        else
        {
            Console.WriteLine("Something went wrong.");
        }
    }

    public List<string> GetDrinkIds()
    {
        return drinkIds;
    }
}