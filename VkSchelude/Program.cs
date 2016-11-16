﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model.RequestParams;
using System.Windows.Forms;
using VkSchelude.Utils;
using System.Text.RegularExpressions;

namespace VkSchelude
{
    class Program
    {
        private static bool _working = true;
        static void Main(string[] args)
        {
            Authorize.setAuthorize();
            //Thread tomorrowScheludeThread = new Thread(Schedule.Start);
            //tomorrowScheludeThread.Start();
            Thread vkBotThread = new Thread(groupBot.Start);
            vkBotThread.Start();
            while (_working)
            {
                var command = Console.ReadLine();
                if (command.Equals("schedule"))
                {
                    //Schedule.buildSchedule(DateTime.Now.AddDays(1).Date.ToString());
                    Send.SendOnWall(Authorize.vkUser, Schedule.buildSchedule(DateTime.Now.AddDays(1).Date.ToString("dd.MM.yyyy")));
                }
                else if (command.Equals("schedule today"))
                    Send.SendOnWall(Authorize.vkUser, Schedule.buildSchedule(DateTime.Now.Date.ToString("dd.MM.yyyy")));
                else if (Regex.IsMatch(command, "^parse"))
                {
                    bool plusConsole = false;
                    string path = "\\rasp.xls";
                    if (command.Contains("+console"))
                        plusConsole = true;
                    if (command.Contains("/filename"))
                    {
                        string fname = command.Substring(command.IndexOf("/filename") + "/filename".Length).Trim();
                        fname = fname.Substring(0, fname.IndexOf(' ') != -1 ? fname.IndexOf(' ') : fname.Length);
                        if (!Regex.IsMatch(fname, "[*\\|:<>?/\"]"))
                        {
                            path = "\\" + fname;
                        }
                        else
                        {
                            Console.WriteLine("Ошибка: значение поля /filename определяется относительно папки рабочего стола текущего пользователя");
                        }
                    }
                    new Document(plusConsole).Parse(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + path);
                }
            }
        }
    }
}
