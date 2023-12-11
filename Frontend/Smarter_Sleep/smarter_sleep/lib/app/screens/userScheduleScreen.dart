import 'package:amplify_flutter/amplify_flutter.dart';
import 'package:flutter/material.dart';
import 'package:smarter_sleep/app/api/api_service.dart';
import '../appFrame.dart';

class UserScheduleScreen extends StatefulWidget {
  const UserScheduleScreen({super.key});

  @override
  State<UserScheduleScreen> createState() => _UserScheduleScreenState();
}

class _UserScheduleScreenState extends State<UserScheduleScreen> {
  List<CustomSchedule> schedule = [];
  List<String> daysOfWeek = [
    'Sunday',
    'Monday',
    'Tuesday',
    'Wednesday',
    'Thursday',
    'Friday',
    'Saturday'
  ];
  TimeOfDay selectedTime = TimeOfDay.now();
  var fetchedTime;
  var timeList = List<TimeOfDay>.filled(7, TimeOfDay(hour: 6, minute: 0));
  var userID;
  var intDate;
  @override
  void initState() {
    super.initState();
    fetchUserSchedule();
  }

  Future<void> fetchUserSchedule() async {
    dynamic response = await ApiService.get('api/CustomSchedules');
    if (response != null) {
      final user = await Amplify.Auth.getCurrentUser();
      userID = user.userId;

      List<CustomSchedule> fetchedSchedules = response
          .where((scheduleData) => scheduleData['userId'] == userID)
          .map<CustomSchedule>((scheduleData) {
        return CustomSchedule.fromJson(scheduleData);
      }).toList();
      setState(() {
        for(int i = 0; i < fetchedSchedules.length; i++){
          timeList[fetchedSchedules[i].dayOfWeek] = fetchedSchedules[i].wakeTime;
        }
      });
    }
    else if (response == null){
      //fetchUserSchedule();
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text("User Schedule"),
        actions: [
          IconButton(
            icon: const Icon(Icons.account_circle),
            onPressed: () {
              mainNavigatorKey.currentState!.pushNamed("/account");
            },
          ),
        ],
      ),
      //TODO: Add some way for the user to submit schedules, send a post request to the api with the new data
      body:
          Column(
            children: <Widget>[
              Padding(
                padding: const EdgeInsets.only(top: 25),
                child: Text("Edit Wake Times Below:", textAlign: TextAlign.center, style: TextStyle(fontSize: 22, fontWeight: FontWeight.bold, color:Color.fromARGB(255, 18, 86, 143))),
              ),
              Expanded(
                child: Padding(
                  padding: const EdgeInsets.all(20.0),
                  child: ListView.builder(
                    itemCount: 7,
                    itemBuilder: (context, index) {
                      final day = daysOfWeek[index];
                      return ListTile(
                        title: Text(day, style: TextStyle(fontSize: 19, fontWeight:FontWeight.bold),),
                        subtitle: Text(timeList[index].format(context), style: TextStyle(fontSize: 18),),
                        trailing: Row(
                          mainAxisSize: MainAxisSize.min,
                          children: [
                            Ink(
                              width: 40,
                              decoration: const ShapeDecoration(
                              color: Colors.blue,
                              shape: CircleBorder()
                             ),
                              child: IconButton(
                                  icon: const Icon(Icons.edit),
                                  color: Colors.white,
                                  iconSize: 25,
                                  onPressed: () async {
                                    final TimeOfDay? pickedTime = await showTimePicker(
                                      context: context,
                                      initialTime: timeList[index],
                                    );
                                    if (pickedTime != null) {
                                      setState(() {
                                        timeList[index] = pickedTime;
                                      });
                                    }
                                  }),
                            ),
                          ],
                        ),
                      );
                    },
                  ),
                ),
              ),
              ClipRRect(
                borderRadius: BorderRadius.circular(3),
                child: Stack(
                children: <Widget>[
                Positioned.fill(
                  child: Container(
                    decoration: const BoxDecoration(
                    color: Colors.blue
                    ),
                  ),
                ),
                ElevatedButton(
                  style: TextButton.styleFrom(
                    foregroundColor: Colors.white,
                    padding: const EdgeInsets.all(10.0),
                    textStyle: const TextStyle(fontSize: 20),
                  ),
                  onPressed: () {
                    _saveAllSchedules();
                    Navigator.pop(context);
                  },
                  child: const Text('Save All Schedules', style: TextStyle(fontWeight:FontWeight.bold),),
                ),
              ],
            ),
              ),
              const SizedBox(height: 20),
            ],
          ),
    );
  }
  void _saveAllSchedules(){
      for (int index = 0; index < daysOfWeek.length; index++){
        var time = timeList[index];
        _saveCustomSchedule(userID, index, time);
    }
   }
Future<void> _saveCustomSchedule(userID, dayOfWeek, wakeTime) async {
  CustomSchedule newSchedule = CustomSchedule(userId: userID, dayOfWeek: dayOfWeek, wakeTime: wakeTime);
  ApiService.put('api/CustomSchedules/${newSchedule.userId}/${newSchedule.dayOfWeek}', newSchedule.toJson())
            .then(
          (response) async {
            fetchUserSchedule();
          },
        );
  }
}


//Can be moved to app/models however only used on this screen
class CustomSchedule {
  final String userId;
  final int dayOfWeek;
  final TimeOfDay wakeTime;

  CustomSchedule({
    required this.userId,
    required this.dayOfWeek,
    required this.wakeTime,
  });

  factory CustomSchedule.fromJson(Map<String, dynamic> json) {
    return CustomSchedule(
      userId: json['userId'],
      dayOfWeek: json['dayOfWeek'] as int,
      wakeTime: TimeOfDay.fromDateTime(
          DateTime.parse("1970-01-01 ${json['wakeTime']}")),
    );
  }

  Map<String, dynamic> toJson() => <String, dynamic>{
        'userId': userId,
        'dayOfWeek': dayOfWeek,
        'wakeTime':
            "${wakeTime.hour.toString().padLeft(2, '0')}:${wakeTime.minute.toString().padLeft(2, '0')}:00",
      };
}