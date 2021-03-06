﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWarriorScheduler
{
    public class Course
    {
        public string subject { get; set; }
        public string catalog_number { get; set; }
        public double units { get; set; }
        public string title { get; set; }
        public string note { get; set; }
        public int class_number { get; set; }
        public string section { get; set; }
        public string campus { get; set; }
        public int associated_class { get; set; }
        public string related_component_1 { get; set; }
        public string related_component_2 { get; set; }
        public int enrollment_capacity { get; set; }
        public int enrollment_total { get; set; }
        public int waiting_capacity { get; set; }
        public int waiting_total { get; set; }
        public object topic { get; set; }
        public List<object> reserves { get; set; }
        public List<Class> classes { get; set; }
        public List<object> held_with { get; set; }
        public int term { get; set; }
        public string academic_level { get; set; }
        public string last_updated { get; set; }

        public string type
        { get { return section.Substring(0, 3); } }

        public bool is_full
        { get { return enrollment_capacity >= enrollment_total ? true : false; } }

        /*private List<string> _weekdays = new List<string>();

        public List<string> weekdays
        { get { return (_weekdays.Count != 0) ? _weekdays : _weekdays = ScheduleHelper.processDate(this.classes[0].date.weekdays); } }

        private DateTime _start_time = DateTime.MinValue;

        public DateTime start_time
        { get { return _start_time == DateTime.MinValue ? _start_time : _start_time = DateTime.ParseExact(this.classes[0].date.start_time, "HH:mm", null); } }

        private DateTime _end_time = DateTime.MinValue;

        public DateTime end_time
        { get { return _end_time == DateTime.MinValue ? _end_time : _end_time = DateTime.ParseExact(this.classes[0].date.end_time, "HH:mm", null); } }

        private string _instructor = "q";

        public string instructor
        { get { return _instructor == "q" ? _instructor : _instructor = this.classes[0].instructors.Count == 0 ? "" : this.classes[0].instructors[0].ToString(); } }

        private string _building = "q";

        public string building
        { get { return _building != "q" ? _building : _building = this.classes[0].location.building; } }

        private string _classroom = "q";

        public string classroom
        { get { return _classroom != "q" ? _classroom : _classroom = this.classes[0].location.building + " " + this.classes[0].location.room; } }

        private float _instructor_rating = -1;

        public float instructor_rating
        { get { return _instructor_rating != -1 ? _instructor_rating : _instructor_rating = ProfessorHelper.getRating(this.instructor); } }*/

        public List<string> weekdays
        { get { return ScheduleHelper.processDate(this.classes[0].date.weekdays); } }

        public DateTime start_time
        { get { return DateTime.ParseExact(this.classes[0].date.start_time, "HH:mm", null); } }

        public DateTime end_time
        { get { return DateTime.ParseExact(this.classes[0].date.end_time, "HH:mm", null); } }

        public string instructor
        { get { return this.classes[0].instructors.Count == 0 ? "" : this.classes[0].instructors[0].ToString(); } }

        public string building
        { get { return this.classes[0].location.building; } }

        public string classroom
        { get { return this.classes[0].location.building + " " + this.classes[0].location.room; } }

        public float instructor_rating
        { get { return ProfessorHelper.getRating(this.instructor); } }
    }
}
