﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Tweetinvi;

namespace ColetorTwitter_Console_CSh
{
    class Program
    {
        static void Main()
        {
            try
            {
                /*Dados para autenticação no twitter*/
                string consumerKey = "add";
                string consumerSecret = "add";
                string userAccessToken = "add";
                string userAccessTokenSecret = "add";
                Auth.SetUserCredentials(consumerKey, consumerSecret, userAccessToken, userAccessTokenSecret);

                /*Nome do servidor, base e coleção do MongoDB onde os dados serão armazenados*/
                string serverMongo = "localhost";
                string dbMongo = "db_twitter";
                string collectionsMongo = "twett";

                /*Dados para autenticação no MongoDB e leitura dos dados no formato da classe "twett"*/
                string conn_mongo = "mongodb://" + serverMongo + "/";
                var client = new MongoClient(conn_mongo);
                var db = client.GetDatabase(dbMongo);
                var col = db.GetCollection<twett>(collectionsMongo);

                /*Coletor de dados do twitter*/
                var stream = Stream.CreateFilteredStream();

                /*Adição das plavras chave para busca dos twetts*/
                stream.AddTrack("moto");
                stream.AddTrack("motocicleta");
                stream.AddTrack("motociclista");
                stream.AddTrack("motoboy");
                stream.AddTrack("viagem de moto");
                stream.AddTrack("honda");
                stream.AddTrack("yamaha");
                stream.AddTrack("kawasaki");
                stream.AddTrack("suzuki");
                stream.AddTrack("harley");
                stream.AddTrack("harley davidson");
                stream.AddTrack("harleydavidson");
                stream.AddTrack("dafra");
                stream.AddTrack("bmw");


                /*Cria um contador para visualizar no console a quantidade de registros coletados*/
                Int64 count = 0;

                /*Coleta o twitt e salva no MongoDB*/
                stream.MatchingTweetReceived += (sender, args) =>
                {
                    /*Incrementa o valor do contador a cada execução*/
                    count++;
                    //Linha abaixo exibe dados do twett no console
                    //Console.WriteLine("Tweet nº {0}:\n{1}\n\n", count.ToString("0000000"), args.Tweet);
                    //Linha abaixo exibe apenas o numero no console, fica a escolha de cada um a forma de exibir a informação no console.
                    Console.WriteLine(count.ToString());
                    //Salva o twett
                    col.InsertOne(new twett(args.Tweet.CreatedAt, args.Tweet.IdStr, args.Tweet.Text, args.Tweet.Language.ToString()));
                };

                stream.StartStreamMatchingAllConditions();

            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
                Console.ReadLine();
            }

        }
    
    }
}
