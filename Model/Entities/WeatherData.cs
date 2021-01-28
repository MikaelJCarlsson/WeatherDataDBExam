using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherDataExam.Model.Entities
{
    public class WeatherData
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public float Temperature { get; set; }
        public float Humidity { get; set; }

        /// <summary>
        /// Sets a starting and end date in the DateRangePicker dynamically based on earliest and last date.
        /// </summary>
        /// <returns></returns>
        public (DateTime,DateTime) SetStartEndDateDateRangePicker()
        {
            using (var context = new WeatherDataDBContext())
            {
                var calendar = context.WeatherData
                    .Select(x => x.Date)
                    .OrderBy(x => x.Date).ToList();

                return (calendar[0].AddDays(-1), calendar[^1]);
            }  
        }  

        /// <summary>
        /// Returns List of Date(s) with a specific location. 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        private static List<WeatherData> DataFromTimeSpan(DateTime start, DateTime end, string location)
        {
            List<WeatherData> avgTemperatures = new List<WeatherData>();
            using (var context = new WeatherDataDBContext())
            {
                if (start == end)
                {
                    var resultSetOneDay = context.WeatherData
                        .Where(x => x.Date >= start && x.Date <= start.AddDays(1) && x.Location == location);

                    foreach (var temperature in resultSetOneDay)
                    {
                        avgTemperatures.Add(temperature);
                    }
                }
                else
                {
                    var resultSet = context.WeatherData
                    .Where(x => x.Date >= start && x.Date <= end && x.Location == location);

                    foreach (var temperature in resultSet)
                    {
                        avgTemperatures.Add(temperature);
                    }
                }
                return avgTemperatures;
            }
        }

        /// <summary>
        /// Returns a list of averages for humidity, temperature and location i.e inside or outside.
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public List<WeatherData> ListOfAvgDate(DateTime start, DateTime end, string location)
        {

            List<WeatherData> linqQuery = DataFromTimeSpan(start, end, location);
            List<WeatherData> avgData = new List<WeatherData>();
            using (var context = new WeatherDataDBContext())
            {
                var sortedQuery = linqQuery
                .GroupBy(x => x.Date.Date)
                .Select(g => new { Date = g.Key, AvgTemp = g.Average(s => (s.Temperature)), AvgHum = g.Average(s => (s.Humidity)) })
                .OrderBy(d => d.Date.Date);

                    foreach (var group in sortedQuery)
                    {
                        WeatherData wd = new WeatherData();
                        wd.Date = group.Date;
                        wd.Temperature = group.AvgTemp;
                        wd.Humidity = group.AvgHum;
                        avgData.Add(wd);
                    }
            }
            return avgData;
        }

        /// <summary>
        /// Determines when Meteorological Autumn and Winter started based on temperatures during a timespan of 5 days
        /// </summary>
        /// <param name="autumn"></param>
        /// <returns></returns>
        public Task<List<WeatherData>> MeteorologicalSeason(bool autumn)
        {
            int daysToCheck = 5;
            int i = 0;
            int j = 4;
            int seasonSelector = autumn ? 8 : 1;
            int tempSelector = autumn ? 10 : 0;
            string stringSeason = autumn ? "Autumn" : "Winter";
            List<WeatherData> datesToBeCompared = new List<WeatherData>();
            List<WeatherData> validDates = new List<WeatherData>();
            using (var context = new WeatherDataDBContext())
            {
                var query = context.WeatherData
                    .Where(x => (x.Date.Month >= seasonSelector && x.Date.Month <= 12) && x.Location == "ute" || x.Date.Month <= 3 && x.Location == "ute")
                    .GroupBy(x => x.Date.Date)
                    .Select(g => new { Date = g.Key, Avg = g.Average(s => s.Temperature), Hum = g.Average(h => h.Humidity) })
                    .OrderBy(x => x.Date);

                foreach (var group in query)
                {
                    WeatherData tmp = new WeatherData();

                    tmp.Date = group.Date;
                    tmp.Temperature = group.Avg;
                    if (group.Avg <= tempSelector)
                    {
                        datesToBeCompared.Add(tmp);
                    }
                    if (datesToBeCompared.Count < 5) //Fill list with 5 starting dates for Timespan to compare
                        continue;

                    try
                    {
                        TimeSpan ts = datesToBeCompared[j].Date - datesToBeCompared[i].Date;
                        if (ts.Days == (daysToCheck - 1))
                        {
                            for (int m = 0; m < datesToBeCompared.Count; m++)
                            {
                                validDates.Add(datesToBeCompared[m]);
                            }
                            break;
                        }
                    }
                    catch (Exception)
                    {
                        datesToBeCompared.Clear();
                        break;
                    }
                    i++;
                    j++;
                }
                return Task.FromResult(validDates);
            }
        }

        /// <summary>
        /// Calculates the risk for mold based on average temperatures and humidity for selected day/days
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        /// 
        public List<WeatherData> MoldRisk(DateTime start, DateTime end ,string location)
        {
            List<WeatherData> moldList = DataFromTimeSpan(start, end, location);

            List<WeatherData> sortedMoldList = new List<WeatherData>();
            using (var context = new WeatherDataDBContext())
            {

                var sortedQuery = moldList
                    .GroupBy(x => x.Date.Date)
                    .Select(g => new { Date = g.Key, AvgTemp = g.Average(s => s.Temperature), AvgHum = g.Average(h => h.Humidity) });

                foreach (var day in sortedQuery)
                {
                    WeatherData wd = new WeatherData();
                    if(day.AvgHum < 75 && day.AvgTemp < 0.0) 
                    {
                        continue;
                    }
                    wd.Date = day.Date;
                    wd.Humidity = day.AvgHum;
                    wd.Temperature = day.AvgTemp;
                    sortedMoldList.Add(wd);
                }
            }
              return sortedMoldList.OrderByDescending(x => (x.Humidity - 78) * (x.Temperature / 15) / 0.22).ToList();
        }

        /// <summary>
        /// Compares previous and current iterations of both inside and outside temperatures to predict how many minutes the balcony door was open
        /// Based on the assumption that if inside temperature drops compared to previous measurement and outside temp is higher than previous measurement
        /// Within a timespan of two or less minutes.
        /// </summary>
        /// <returns></returns>
        public List<Values> BalconyDoor()
        {
            bool isOpen = false;
            DateTime doorOpened = DateTime.Now;
            DateTime doorClosed = DateTime.Now;
            int totalOpenTime = 0;

            List<Values> doorOpenMinutes = new List<Values>();
            using (var context = new WeatherDataDBContext())
            {
                var query = context.WeatherData
                    .OrderBy(x => x.Date)
                    .AsEnumerable()
                    .GroupBy(x => x.Date.Date).ToList();

                bool startReadingInside = true;
                bool startReadOutside = true;
                float previousTempInside = 0;
                float insideDiff = 0;
                float outsideDiff = 0;
                float previousTempOutside = 0;

                DateTime previousTimeInside = DateTime.Now;
                DateTime previousTime = DateTime.Now;

                foreach (var day in query)
                {
                    Values wd = new Values();

                    foreach (var measurement in day)
                    {
                        if (startReadingInside && measurement.Location == "Inne")
                        {
                            previousTempInside = measurement.Temperature;
                            previousTimeInside = measurement.Date;
                            previousTime = measurement.Date;
                            startReadingInside = false;
                            continue;
                        }
                        if (startReadOutside && measurement.Location == "Ute")
                        {
                            previousTime = measurement.Date;
                            previousTempOutside = measurement.Temperature;
                            startReadOutside = false;
                            continue;
                        }

                        if (measurement.Location == "Inne")
                        {
                            insideDiff = previousTempInside - measurement.Temperature;
                        }
                        if (measurement.Location == "Ute")
                        {
                            outsideDiff = previousTempOutside - measurement.Temperature;
                        }

                        TimeSpan deltaT = measurement.Date - previousTimeInside;  //deltaT controlls that if measurements are too far apart in time, door closes at previous timestamp.

                        if (isOpen == false && insideDiff > 0 && outsideDiff < 0) // checks previous - current temp to see if conditons are met to open door.
                        {
                            doorOpened = previousTimeInside;
                            isOpen = true;
                        }

                        if (isOpen == true && deltaT.TotalMinutes > 2) //closes door if too far apart between measurements in time.
                        {
                            doorClosed = previousTimeInside;
                            isOpen = false;
                            TimeSpan deltaY = doorClosed - doorOpened;
                            totalOpenTime += deltaY.Minutes;
                            previousTempInside = measurement.Temperature;
                            previousTimeInside = measurement.Date;
                            continue;
                        }
                        if (measurement.Location == "Inne" && isOpen == true && measurement.Temperature > previousTempInside) // If temperature rises i guess that door is closed?
                        {
                            doorClosed = measurement.Date;
                            isOpen = false;
                            TimeSpan deltaY = doorClosed - doorOpened;
                            totalOpenTime += deltaY.Minutes;
                        }

                        if (measurement.Location == "Inne")
                        {
                            previousTempInside = measurement.Temperature;
                            previousTimeInside = measurement.Date;
                        }

                        if (measurement.Location == "Ute")
                        {
                            previousTempOutside = measurement.Temperature;
                        }
                        previousTime = measurement.Date;
                    }
                     
                    wd.DoorOpenMinutes = totalOpenTime;
                    wd.Date = day.Key;
                    doorOpenMinutes.Add(wd);
                    totalOpenTime = 0;

                    //results seem strange but debugging shows correct statements were met.
                }
            }

            var doorOpenMinutesDay = doorOpenMinutes
               .AsEnumerable()
               .OrderByDescending(x => x.DoorOpenMinutes)
               .Take(10).ToList();

            return doorOpenMinutesDay;
        }

        /// <summary>
        /// Calculates The Maxium and Minimum temperature differences each day and returns 10 days with the highest total difference i.e Max Difference - Min difference
        /// </summary>
        /// <returns></returns>
        public List<Values> TempDiffPerDayMinMax()
        {
            List<Values> minMaxValues = new List<Values>();
            List<Values> tempData = new List<Values>();
            using (var context = new WeatherDataDBContext())
            {
                var query = context.WeatherData
                .AsEnumerable()
                .OrderBy(x => x.Date)
                .GroupBy(x => x.Date.Date);

                foreach (var day in query)
                {
                    List<WeatherData> insideList = new List<WeatherData>();
                    List<WeatherData> outsideList = new List<WeatherData>();
                    foreach (var entry in day)
                    {
                        if (entry.Location == "Inne")
                            insideList.Add(entry);
                        else
                            outsideList.Add(entry);
                    }
                    float inMin = insideList.Min(x => x.Temperature); 
                    float inMax = insideList.Max(x => x.Temperature);
                    float outMin = outsideList.Min(x => x.Temperature);
                    float outMax = outsideList.Max(x => x.Temperature);

                    float diffOne = Math.Abs(outMax - inMin); //Check span between temps
                    float diffTwo = Math.Abs(inMax - outMin);

                    float minDist = 100000; //Huge problem  IRL if minDist is exceeded
                    float tmp;
                    for (int i = 0; i < insideList.Count; i++)
                    {
                        for (int j = 0; j < outsideList.Count; j++)
                        {
                            tmp = Math.Abs(insideList[i].Temperature - outsideList[j].Temperature); //Sorting 
                            if (minDist > tmp)
                                minDist = tmp;
                        }
                    }

                    Values tD = new Values();
                    tD.Date = day.Key.Date;
                    tD.MinDiff = minDist;

                    if (diffOne > diffTwo)
                        tD.MaxDiff = diffOne; //Select wich span was greater between lists.
                    else
                        tD.MaxDiff = diffTwo;

                    tempData.Add(tD);
                    insideList.Clear();
                    outsideList.Clear();
                }
                var q1 = tempData
                .OrderByDescending(x => Math.Abs(x.MaxDiff - x.MinDiff));
                foreach (var day in q1)
                {
                    Values tD = new Values();
                    tD.Date = day.Date;
                    tD.MaxDiff = day.MaxDiff;
                    tD.MinDiff = day.MinDiff;
                    minMaxValues.Add(tD);
                } //sort, add, return.
                return minMaxValues.Take(10).ToList();
            }
        }
    }
}
