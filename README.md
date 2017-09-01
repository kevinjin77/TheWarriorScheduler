# TheWarriorScheduler
An application to find the best possible schedules for Waterloo students. (WIP)

<br/>

## TODO:
#### Add Rating System for generated schedule:
   - ~~Professor Rating~~ **✓**
   - ~~Distance Rating (Based on distance between buildings using API to get lat/longitude and then using Google Maps)~~ **✓**
   - ~~Gap Rating (Based on number of small gaps)~~ **✓**
   - ~~Lunch Rating (Based on time allocated for lunch)~~ **✓**
   - ~~Early Bird/Night Owl Filter (No 8:30/Night Classes, if possible)~~ **✓**
   - Early Bird/Night Owl Rating
   - ~~Proximity Rating (Least number of changes from current schedule, if given one)~~ **✓**
   - Scale Individual Ratings out of 100, scale overall ratings based on importance (Professor Rating > Gap Rating)

#### General Stuff:
   - Include TUT, TST in Schedule
   - *Accomodate ENG Classes (One Lecture, Multiple Classes)*
      - Implemented, but extremely slow due to sheer amount of combinations. Need to look into optimizing algorithm.
   - Show Open Classes Only (Or Blue Square if class if full, eventually take reserves into account)
   - ~~Accomodate Online Classes (No Start Time, End Time, Weekdays)~~ **✓**
      - Still having issues with SCI 238
   - Add Filters for Online Classes, Off-Campus Classes (Conrad Grebel, St.Paul's, etc.)

<br/>

#### Known Defects:
   - ~~Handle case where building is not found. (ex. SJ1)~~ **✓**
