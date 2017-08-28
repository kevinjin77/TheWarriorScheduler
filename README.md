# TheWarriorScheduler
An application to find the best possible schedules for Waterloo students. (WIP)

<br/>

## TODO:
#### Add Rating System for generated schedule:
   - ~~Professor Rating~~ **✓**
   - *Distance Rating (Based on distance between buildings using API to get lat/longitude and then using Google Maps)*
      - Works, but is too slow.
   - ~~Gap Rating (Based on number of small gaps)~~ **✓**
   - ~~Lunch Rating (Based on time allocated for lunch)~~ **✓**
   - ~~Early Bird/Night Owl Filter (No 8:30/Night Classes, if possible)~~ **✓**
   - Early Bird/Night Owl Rating
   - Proximity Rating (Least number of changes from current schedule, if given one)
   - Scale Individual Ratings out of 100, scale overall ratings based on importance (Professor Rating > Gap Rating)
   
<br/>

#### General Stuff:
   - Include TUT, TST in Schedule
   - Accomodate ENG Classes (One Lecture, Multiple Classes)
   - Show Open Classes Only (Or Blue Square if class if full, eventually take reserves into account)
