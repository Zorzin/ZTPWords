using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZTPwords.Models;

namespace ZTPwords.Logic.UserLevel
{
    public class LevelChecker
    {
        public void CheckLevel(ApplicationUser user, double points)
        {
            double maxPointsOnLevel = 0;
            if (user.Level<5)
            {
                maxPointsOnLevel = 80;
            }
            else if (user.Level <10)
            {
                maxPointsOnLevel = 120;
            }
            else if(user.Level>=10)
            {
                maxPointsOnLevel = 200;
            }

            var userPoints = user.Points;
            userPoints = userPoints + points;
            if (userPoints>=maxPointsOnLevel)
            {
                user.Level++;
                userPoints = userPoints - maxPointsOnLevel;
            }
            user.Points = userPoints;
        }
    }
}