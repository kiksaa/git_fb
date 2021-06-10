/* Calendar */
/*-------- */

var Calendar = FullCalendar.Calendar;
var Draggable = FullCalendarInteraction.Draggable;
var containerEl = document.getElementById('external-events');
var calendarEl = document.getElementById('fc-external-drag');
var checkbox = document.getElementById('drop-remove');
$(document).ready(function () {
    /*debugger*/

    GenerateCalender();

    $('#external-events .fc-event').each(function () {
        // Different colors for events
        $(this).css({ 'backgroundColor': $(this).data('color'), 'borderColor': $(this).data('color') });
    });
    var colorData;
    $('#external-events .fc-event').mousemove(function () {
        colorData = $(this).data('color');
    })
    // Draggable event init
    new Draggable(containerEl, {
        itemSelector: '.fc-event',
        eventData: function (eventEl) {
            return {
                title: eventEl.innerText,
                color: colorData
            };
        }
    });
})
function GenerateCalender() {
    /*debugger*/
    var d = new Date();

    var month = d.getMonth() + 1;
    var day = d.getDate();

    var output = d.getFullYear() + '-' +
        (month < 10 ? '0' : '') + month + '-' +
        (day < 10 ? '0' : '') + day;

    //var basicCal = document.getElementById('basic-calendar');
    //var fcCalendar = new FullCalendar.Calendar(basicCal, {
    //    header: {
    //        left: 'prev,next today',
    //        center: 'title',
    //        right: 'month,basicWeek,basicDay'
    //    },

    //    editable: true,
    //    //droppable: true,
    //    //defaultDate: output,

    //    plugins: ["dayGrid", "timeGrid", "interaction"],
    //    eventLimit: true, // allow "more" link when too many events
    //    events: eventsDisplay,
    //    viewRender: function (view, element) {

    //        $('.fc-center')[0].children[0].innerText = view.title.replace(new RegExp("undefined", 'g'), "");
    //    },
    //    eventDataTransform: function (event) {

    //        if (event.allDay) {
    //            event.end = moment(event.end).add(1, 'days')
    //        }
    //        return event;
    //    },
    //    eventClick: function (calEvent, jsEvent, view) {
    //        debugger
    //        //Start = calEvent.start.format('MM/DD/YYYY HH:mm');
    //        //End = calEvent.end != null ? calEvent.end.format('MM/DD/YYYY') + ' 00:00:00' : null;
    //        var id = calEvent.el.innerText;
    //        id = id.split(/\s+/)
    //        if (id[0] != null) {
    //            var ticketId = parseFloat(id[0]);
    //            if (isNaN(ticketId)) {

    //            }
    //            else {
    //                window.open("/Tickets/viewticket?Id=" + ticketId, '_blank');
    //            }
    //        }
    //    },
    //});
    //fcCalendar.render();

    var calendar = new Calendar(calendarEl, {
        header: {
            left: 'prev,next today',
            center: 'title',
            right: "dayGridMonth,timeGridWeek,timeGridDay"
        },
        editable: true,
        plugins: ["dayGrid", "timeGrid", "interaction"],
        droppable: true,
        defaultDate: output,
        events: [
            {
                title: 'All Day Event',
                start: '2020-05-01'
            },
            {
                title: 'Long Event',
                start: '2020-05-07',
                end: '2020-05-10'
            },
            {
                id: 999,
                title: 'Repeating Event',
                start: '2020-05-09T16:00:00'
            },
            {
                id: 999,
                title: 'Repeating Event',
                start: '2020-01-16T16:00:00'
            },
            {
                title: 'Conference',
                start: '2020-01-11',
                end: '2020-01-13'
            },
            {
                title: 'Meeting',
                start: '2020-05-12T10:30:00',
                end: '2020-05-12T12:30:00'
            },
            {
                title: 'Dinner',
                start: '2020-01-12T20:00:00'
            },
            {
                title: 'Birthday Party',
                start: '2020-05-13T07:00:00'
            },
            {
                title: 'Click for Google',
                url: 'http://google.com/',
                start: '2020-05-28'
            }
        ],
        //events: [
        //    {
        //        title: 'test | null | 05:08 PM-01:08 PM',
        //        start: '2020-03-03T05:08:00',
        //        end: '2020-03-03T01:08:00',

        //        color: "#ff5722",
        //        description: ''
        //    }
        //    // more events ...
        //],
        drop: function (info) {
            // is the "remove after drop" checkbox checked?
            if (checkbox.checked) {
                // if so, remove the element from the "Draggable Events" list
                info.draggedEl.parentNode.removeChild(info.draggedEl);
            }
        },
        viewRender: function (view, element) {
            $('.fc-center')[0].children[0].innerText = view.title.replace(new RegExp("undefined", 'g'), "");
        },
        eventDataTransform: function (event) {
            if (event.allDay) {
                event.end = moment(event.end).add(1, 'days')
            }
            return event;
        },
        eventClick: function (calEvent, jsEvent, view) {
            debugger
            //Start = calEvent.start.format('MM/DD/YYYY HH:mm');
            //End = calEvent.end != null ? calEvent.end.format('MM/DD/YYYY') + ' 00:00:00' : null;
            var events = calEvent.event._def.extendedProps;
            //if (events.eventID != 0) {
            //    window.open("/Tickets/viewticket?Id=" + calEvent.event._def.extendedProps.eventID, '_blank');
            //}
            //else {

            //}
            //var id = calEvent.el.innerText;
            //id = id.split(/\s+/)
            //if (id[3] != null) {
            //    if (id[3]!="null") {
            //        var ticketId = parseFloat(id[3]);
            //        if (isNaN(ticketId)) {

            //        }
            //        else {
            //            window.open("/Tickets/viewticket?Id=" + ticketId, '_blank');
            //        }
            //    }
            //}
        },
    });

    calendar.render();
    //var calendar = $('#calendar').fullCalendar({
    //    header: {
    //        left: 'prev,next today',
    //        center: 'title',
    //        right: 'month,basicWeek,basicDay'
    //    },
    //    timeFormat: 'h(:mm)a',
    //    editable: true,
    //    droppable: true,
    //    events: events,
    //    viewRender: function (view, element) {

    //        $('.fc-center')[0].children[0].innerText = view.title.replace(new RegExp("undefined", 'g'), "");
    //    },
    //    drop: function (date, jsEvent, ui, resourceId) {

    //        //$("#CrewModel").modal("open");
    //        Crew = this;
    //        hdnCrewId = parseInt($(this).attr('id').toString());
    //        hdnGroupCalId = 0;
    //        $("#lblddl_JobPriority").addClass("active");
    //    },
    //    eventDragStop: function (event, jsEvent, ui, view) {

    //    },
    //    eventReceive: function (event) {
    //        alert('hello');
    //        //alert(JSON.stringify(event, null, 4));

    //        Start = event.start.format('MM/DD/YYYY');  //format('DD/MM/YYYY HH:mm A');
    //        End = event.end != null ? event.end.format('MM/DD/YYYY HH:mm').add(1, 'days') : null;
    //        $("#txtCrew").val(event.title)
    //        $("#CrewModel").modal("open");
    //        $("#hdn").val(event._id)
    //    },
    //    //eventRender: function (event, element, view) {

    //    //    $(".a").append("A,");
    //    //},
    //    eventDrop: function (event) { // called when an event (already on the calendar) is moved
    //        alert();
    //        hdnGroupCalId = event.GroupCalID;
    //        var CrewId = event.eventID;
    //        var JobId = event.JobId;
    //        var Time = event.Time;
    //        var StartDate = event.start.format('MM/DD/YYYY');
    //        var EndDate = event.end != null ? moment(event.end).subtract('days', 1).format('MM/DD/YYYY') : null;
    //        //var EndDate = event.end.format('MM/DD/YYYY').subtract('days', 1);
    //        //var EndDate = event.end != null ? event.end.format('MM/DD/YYYY') + ' 00:00:00' : null;
    //        //var EndDate = event.end != null ?moment(event.end).format('MM/DD/YYYY HH:mm').subtract('days', 1) : null;
    //        var ToHRS = event.ToHRS;
    //        var TotalHRS = event.TotalHRS;
    //        var JobPriority = event.JobPriority;
    //        IsresizeOrDrag = true;
    //        EventSave(CrewId, JobId, Time, StartDate, EndDate, ToHRS, TotalHRS, JobPriority);

    //    },
    //    eventDataTransform: function (event) {

    //        if (event.allDay) {
    //            event.end = moment(event.end).add(1, 'days')
    //        }
    //        return event;
    //    },
    //    eventResize: function (event, delta, revertFunc) {

    //        //$("#txtCrew").val(event.title)
    //        //$("#hdn").val(event._id)

    //        //revertFunc();

    //        hdnGroupCalId = event.GroupCalID;
    //        var CrewId = event.eventID;
    //        var JobId = event.JobId;
    //        var Time = event.Time;
    //        var StartDate = event.start.format('MM/DD/YYYY');
    //        //var EndDate = event.end != null ? event.end.format('MM/DD/YYYY') + ' 00:00:00' : null;
    //        var EndDate = event.end != null ? moment(event.end).subtract('days', 1).format('MM/DD/YYYY') : null;

    //        //
    //        var ToHRS = event.ToHRS;
    //        var TotalHRS = event.TotalHRS;
    //        var JobPriority = event.JobPriority;
    //        IsresizeOrDrag = true;
    //        EventSave(CrewId, JobId, Time, StartDate, EndDate, ToHRS, TotalHRS, JobPriority)

    //    },
    //    eventClick: function (calEvent, jsEvent, view) {

    //        Start = calEvent.start.format('MM/DD/YYYY HH:mm');
    //        End = calEvent.end != null ? calEvent.end.format('MM/DD/YYYY') + ' 00:00:00' : null;

    //        hdnGroupCalId = calEvent.GroupCalID;
    //        $("#ddl_Job").val(calEvent.JobId);
    //        hdnCrewId = calEvent.eventID;
    //        $("#txtFromDate").val(calEvent.start.format('MM/DD/YYYY'));
    //        $("#txtToDate").val(calEvent.EndD.format('MM/DD/YYYY'));

    //        $("#txtCrew").val(calEvent.Subject);
    //        $("#txtTime").val(calEvent.Time);

    //        $("#txtToHRS").val(calEvent.ToHRS);
    //        $("#txtTotalHRS").val(calEvent.TotalHRS);
    //        $('#ddl_Job').material_select();
    //        $("#ddl_Job").prop("disabled", true);

    //        $("#ddl_JobPriority").val(calEvent.JobPriority);
    //        $('#ddl_JobPriority').material_select();
    //        $("#txtFromDate").next().addClass("active");
    //        $("#txtToDate").next().addClass("active");
    //        $("#txtTime").next().addClass("active");
    //        $("#txtToHRS").next().addClass("active");
    //        $('#ddl_Job').material_select();

    //        $("#lblddl_JobPriority").addClass("active");
    //        $("#CrewModel").modal("open");
    //        //var CrewId = parseInt(hdnCrewId);
    //        //var JobId = parseInt($("#ddl_Job").val());
    //        //var Time = $("#txtTime").val();
    //        //var StartDate = Start;
    //        //var EndDate = End;

    //        //EventSave(CrewId, JobId, Time, StartDate, EndDate)



    //    },
    //    //,eventDragStop: function (event, jsEvent, ui, view) {
    //    //
    //    //    $('#calendar').fullCalendar('removeEvents', event._id);
    //    //}

    //});
}
//$(document).ready(function () {
//  /* initialize the calendar
//   * al 
//   -----------------------------------------------------------------*/
//    debugger
    
//  var Calendar = FullCalendar.Calendar;
//  var Draggable = FullCalendarInteraction.Draggable;
//  var containerEl = document.getElementById('external-events');
//  var calendarEl = document.getElementById('fc-external-drag');
//  var checkbox = document.getElementById('drop-remove');

//  //  Basic Calendar Initialize
//  var basicCal = document.getElementById('basic-calendar');
//    var d = new Date();

//    var month = d.getMonth() + 1;
//    var day = d.getDate();

//    var output = d.getFullYear() + '-' +
//        (month < 10 ? '0' : '') + month + '-' +
//        (day < 10 ? '0' : '') + day;
//    var fcCalendar = new FullCalendar.Calendar(basicCal, {

//        defaultDate: output,
//    editable: true,
//    plugins: ["dayGrid", "interaction"],
//    eventLimit: true, // allow "more" link when too many events
//    events: [
//      {
//        title: 'All Day Event',
//        start: '2020-05-01'
//      },
//      {
//        title: 'Long Event',
//        start: '2020-05-07',
//        end: '2020-05-10'
//      },
//      {
//        id: 999,
//        title: 'Repeating Event',
//        start: '2020-05-09T16:00:00'
//      },
//      {
//        id: 999,
//        title: 'Repeating Event',
//        start: '2020-01-16T16:00:00'
//      },
//      {
//        title: 'Conference',
//        start: '2020-01-11',
//        end: '2020-01-13'
//      },
//      {
//        title: 'Meeting',
//        start: '2020-05-12T10:30:00',
//        end: '2020-05-12T12:30:00'
//      },
//      {
//        title: 'Dinner',
//        start: '2020-01-12T20:00:00'
//      },
//      {
//        title: 'Birthday Party',
//        start: '2020-05-13T07:00:00'
//      },
//      {
//        title: 'Click for Google',
//        url: 'http://google.com/',
//        start: '2020-05-28'
//      }
//    ],
//  });
//  fcCalendar.render();

//  // initialize the calendar
//  // -----------------------------------------------------------------
//  var calendar = new Calendar(calendarEl, {
//    header: {
//      left: 'prev,next today',
//      center: 'title',
//      right: "dayGridMonth,timeGridWeek,timeGridDay"
//    },
//    editable: true,
//    plugins: ["dayGrid", "timeGrid", "interaction"],
//    droppable: true, // this allows things to be dropped onto the calendar
//    defaultDate: '2019-01-01',
//    events: [
//      {
//        title: 'All Day Event',
//        start: '2019-01-01',
//        color: '#009688'
//      },
//      {
//        title: 'Long Event',
//        start: '2019-01-07',
//        end: '2019-01-10',
//        color: '#4CAF50'
//      },
//      {
//        id: 999,
//        title: 'Meeting',
//        start: '2019-01-09T16:00:00',
//        color: '#00bcd4'
//      },
//      {
//        id: 999,
//        title: 'Happy Hour',
//        start: '2019-01-16T16:00:00',
//        color: '#3f51b5'
//      },
//      {
//        title: 'Conference Meeting',
//        start: '2019-01-11',
//        end: '2019-01-13',
//        color: '#e51c23'
//      },
//      {
//        title: 'Meeting',
//        start: '2019-01-12T10:30:00',
//        end: '2019-01-12T12:30:00',
//        color: '#00bcd4'
//      },
//      {
//        title: 'Dinner',
//        start: '2019-01-12T20:00:00',
//        color: '#4a148c'
//      },
//      {
//        title: 'Birthday Party',
//        start: '2019-01-13T07:00:00',
//        color: '#ff5722'
//      },
//      {
//        title: 'Click for Google',
//        url: 'http://google.com/',
//        start: '2019-01-28',
//      }
//    ],
//    drop: function (info) {
//      // is the "remove after drop" checkbox checked?
//      if (checkbox.checked) {
//        // if so, remove the element from the "Draggable Events" list
//        info.draggedEl.parentNode.removeChild(info.draggedEl);
//      }
//    }
//  });
//  calendar.render();

//  // initialize the external events
//  // ----------------------------------------------------------------

//  //   var colorData;
//  $('#external-events .fc-event').each(function () {
//    // Different colors for events
//    $(this).css({ 'backgroundColor': $(this).data('color'), 'borderColor': $(this).data('color') });
//  });
//  var colorData;
//  $('#external-events .fc-event').mousemove(function () {
//    colorData = $(this).data('color');
//  })
//  // Draggable event init
//  new Draggable(containerEl, {
//    itemSelector: '.fc-event',
//    eventData: function (eventEl) {
//      return {
//        title: eventEl.innerText,
//        color: colorData
//      };
//    }
//  });
//})