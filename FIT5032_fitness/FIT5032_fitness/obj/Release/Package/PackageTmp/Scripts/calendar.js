var events = [];
$(".booking").each(function () {
    var title = "Training Group";
    var email = $(".email", this).text().trim();
    var start = $(".start", this).text().trim();
    var selectedDate = new Date(start).getDay();

    var event = {
        "title": title,
        "start": start,
        "description": email
    };
    
    console.log(event);
    events.push(event);
});

var calendar = $('#calendar');
calendar.fullCalendar({
    locale: 'au',
    selectable: true,
    header: {
        left: 'prev,next today',
        center: 'title',
        right: 'month,agendaWeek,agendaDay'
    },
    events: events,
    //events: [
    //    {
    //        title: 'Test',
    //        start: '2018-09-16 14:30:00',
    //        end: '2018-09-16 16:30:00',
    //        allDay: false
    //    }
    //],
    //timeFormat: 'H(:mm)',
    eventRender: function (event, element) {
        element.find(".fc-title").append(" (" + event.description + ")");
    },
    eventColor: '#378006',
    displayEventTime: false,
    dayClick: function (date, allDay, jsEvent, view) {
        var d = new Date(date);
        var m = moment(d).format("YYYY-MM-DD");
        m = encodeURIComponent(m);
        var todayDate = moment($.now()).format("YYYY-MM-DD");
        var currentDate = d.getDay();
        if (m < todayDate ) {
            alert("Sorry, you can't select the date in the past");
        }
        if (m >= todayDate) {
            if (currentDate === 2 || currentDate === 4) {
                var uri = "/GroupTrainings/Create?date=" + m;
                $(location).attr('href', uri);
            }
            else {
               alert("Please select Tuesday and Thursday only");
            }
        }
    }
});

