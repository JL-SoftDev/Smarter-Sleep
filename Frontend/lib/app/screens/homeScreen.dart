import 'dart:async';
import 'package:amplify_flutter/amplify_flutter.dart';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:smarter_sleep/app/api/api_service.dart';
import 'package:smarter_sleep/app/models/survey.dart';

import 'package:smarter_sleep/app/models/user_challenge.dart';
import 'package:smarter_sleep/app/models/sleep_review.dart';
import 'package:smarter_sleep/app/models/wearable_log.dart';
import 'package:smarter_sleep/main.dart';

import 'surveyFormScreen.dart';
import '../appFrame.dart';

class HomeScreen extends StatefulWidget {
  const HomeScreen({super.key});

  @override
  State<HomeScreen> createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  final GlobalServices _globalServices = GlobalServices();

  List<UserChallenge> userChallenges = [];
  bool isSleeping = false;
  late Stopwatch sleepTime;
  late Timer updateTime;
  int? _sleepScore;
  late String userId;

  final List<Color> barColors = [
    /// Zzz Zoom
    Colors.indigo.shade400,

    /// Dreamy Eight
    Colors.blue.shade700,

    /// Midnight Munch Ban
    Colors.deepPurple.shade700,

    /// Dynamic Dream Duo
    Colors.lightGreen,

    /// Seamless Sleep Automation
    Colors.orange.shade300,

    /// Sunrise Scheduler
    Colors.yellow.shade300,
  ];

  final List<Color> backgroundColors = [
    /// Zzz Zoom
    Colors.indigo.shade100,

    /// Dreamy Eight
    Colors.blue.shade200,

    /// Midnight Munch Ban
    Colors.deepPurple.shade300,

    /// Dynamic Dream Duo
    Colors.lightGreen.shade300,

    /// Seamless Sleep Automation
    Colors.orange.shade100,

    /// Sunrise Scheduler
    Colors.yellow.shade100,
  ];

  int colorIndex = 0;

  @override
  void initState() {
    super.initState();

    sleepTime = Stopwatch();
    updateTime = Timer.periodic(const Duration(minutes: 1), (timer) {
      setState(() {});
    });

    _initializeUser();
  }

  @override
  void dispose() {
    updateTime.cancel();
    super.dispose();
  }

  Future<List<UserChallenge>> _fetchChallenges(String userId) async {
    dynamic challengesResponse = await ApiService.get(
        'api/UserChallenges/progress?userId=${userId}&dateTime=${_globalServices.currentTime}');
    List<UserChallenge> fetchedChallenges = [];
    if (challengesResponse != null) {
      fetchedChallenges = challengesResponse
          .map<UserChallenge>((json) => UserChallenge.fromJson(json))

          /// Filter challenges that don't begin for another day or more.
          .where((UserChallenge chl) => chl.startDate
              .isBefore(_globalServices.currentTime.add(Duration(days: 1))))
          .toList();

      fetchedChallenges.sort((a, b) => b.startDate.compareTo(a.startDate));
      return fetchedChallenges;
    }
    return [];
  }

  Future<int> _fetchSleepScore(String userId) async {
    dynamic response = await ApiService.get('api/SleepReviews');

    if (response != null) {
      List<SleepReview> reviews = response
          .where((reviewData) =>
              reviewData['userId'] == userId &&
              reviewData['survey'] != null &&
              reviewData['wearableLog'] != null)
          .map<SleepReview>((json) => SleepReview.fromJson(json))
          .toList();
      if (reviews.isNotEmpty) {
        SleepReview lastReview =
            reviews.reduce((a, b) => a.createdAt.isAfter(b.createdAt) ? a : b);
        return lastReview.smarterSleepScore;
      }
    }
    return 0;
  }

  Future<void> _initializeUser() async {
    final user = await Amplify.Auth.getCurrentUser();

    List<UserChallenge> fetchedChallenges = await _fetchChallenges(user.userId);
    int fetchedSleepScore = await _fetchSleepScore(user.userId);

    setState(() {
      userId = user.userId;
      userChallenges = fetchedChallenges;
      _sleepScore = fetchedSleepScore;
    });
  }

  //Currently always returns a wearable log, implemented to support no wearable data found.
  Future<WearableLog?> _fetchWearableData(bool goodData) async {
    WearableLog useDefaultLog() {
      DateTime lastNight = _globalServices.currentTime
          .subtract(const Duration(days: 1))
          .add(const Duration(hours: 21));
      DateTime wakeTime = lastNight.add(const Duration(hours: 8));

      return WearableLog(
          sleepStart: lastNight,
          sleepEnd: wakeTime,
          hypnogram:
              "443432222211222333321112222222222111133333322221111223333333333223222233222111223333333333332224",
          sleepScore: 100,
          sleepDate: DateFormat('yyyy-MM-dd').format(lastNight));
    }

    try {
      dynamic response = await ApiService.get(
          'api/WearableDataInjection/${goodData ? 'better' : 'worse'}?UserId=$userId&dateTime=${_globalServices.currentTime}');

      if (response != null) {
        WearableLog data = WearableLog.fromJson(response);
        return data;
      } else {
        // No wearable log found
        return useDefaultLog();
      }
    } catch (e) {
      return useDefaultLog();
    }
  }

  Future<void> _popupReview(SleepReview review) async {
    return showDialog(
        context: context,
        builder: (BuildContext context) {
          return AlertDialog(
            title: const Text('Sleep Review Data'),
            content: SingleChildScrollView(
                child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text('Created At: ${review.createdAt.toString()}'),
                Text('Smarter Sleep Score: ${review.smarterSleepScore}'),
                const SizedBox(height: 10),
                const Text('Survey Details:'),
                Text('  Sleep Quality: ${review.survey.sleepQuality}'),
                Text('  Wake Preference: ${review.survey.wakePreference}'),
                Text(
                    '  Temperature Preference: ${review.survey.temperaturePreference}'),
                Text(
                    '  Lights Disturbance: ${review.survey.lightsDisturbance}'),
                Text('  Sleep Earlier: ${review.survey.sleepEarlier}'),
                Text('  Ate Late: ${review.survey.ateLate}'),
                Text('  Sleep Duration: ${review.survey.sleepDuration}'),
                Text('  Survey Date: ${review.survey.surveyDate}'),
                const SizedBox(height: 10),
                const Text('Wearable Details:'),
                Text(
                    '  Sleep Start: ${review.wearableLog.sleepStart.toString()}'),
                Text('  Sleep End: ${review.wearableLog.sleepEnd.toString()}'),
                Text('  Hypnogram: ${review.wearableLog.hypnogram}'),
                Text('  Sleep Score: ${review.wearableLog.sleepScore}'),
                Text('  Sleep Date: ${review.wearableLog.sleepDate}'),
              ],
            )),
            actions: <Widget>[
              TextButton(
                onPressed: () => Navigator.pop(context, 'OK'),
                child: const Text('OK'),
              ),
            ],
          );
        });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text("Smarter Sleep Home"),
        actions: [
          IconButton(
            icon: const Icon(Icons.schedule),
            onPressed: () {
              mainNavigatorKey.currentState!
                  .pushNamed("/schedule")
                  .then((_) async {
                /// Check if user is assigned sunrise scheduler
                if (userChallenges.any((chl) => chl.challengeId == 6)) {
                  List<UserChallenge> fetchedChallenges =
                      await _fetchChallenges(userId);
                  setState(() {
                    userChallenges = fetchedChallenges;
                  });
                }
              });
            },
          ),
          IconButton(
            icon: const Icon(Icons.account_circle),
            onPressed: () {
              mainNavigatorKey.currentState!.pushNamed("/account");
            },
          ),
        ],
      ),
      body: Center(
        child: Column(
          children: <Widget>[
            FractionallySizedBox(
              widthFactor: 1.0,
              child: Container(
                height: MediaQuery.of(context).size.height * 0.28,
                color: isSleeping ? Colors.purple[900] : Colors.blue[800],
                child: Column(
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: <Widget>[
                    Text(
                      isSleeping ? "Sleeping" : "Smarter Sleep Score",
                      style: const TextStyle(
                        color: Colors.white,
                        fontSize: 24,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                    const SizedBox(height: 40),
                    isSleeping
                        ? Text(
                            '${sleepTime.elapsed.inHours.toString().padLeft(2, "0")}H:${(sleepTime.elapsed.inMinutes % 60).toString().padLeft(2, "0")}M',
                            style: const TextStyle(
                                fontSize: 40, color: Colors.white))
                        : Stack(
                            alignment: Alignment.center,
                            children: <Widget>[
                              Transform.scale(
                                scale: 2.5,
                                child: CircularProgressIndicator(
                                  value: (_sleepScore ?? 100) / 100,
                                  valueColor:
                                      const AlwaysStoppedAnimation<Color>(
                                          Colors.white),
                                ),
                              ),
                              Text(_sleepScore?.toString() ?? '--',
                                  style: const TextStyle(
                                      fontSize: 40, color: Colors.white)),
                            ],
                          ),
                  ],
                ),
              ),
            ),
            const SizedBox(height: 20),
            const Text(
              "User Challenges",
              style: TextStyle(
                fontSize: 24,
                fontWeight: FontWeight.bold,
              ),
            ),
            const SizedBox(height: 10),
            Column(
              children: List.generate(4, (index) {
                if (index < userChallenges.length) {
                  UserChallenge chl = userChallenges[index];

                  /// -1 : Expired, 0 : In Progress, 1 : Completed
                  int status = (chl.numCompleted >= chl.numTargeted)
                      ? 1
                      : (chl.expireDate != null
                          ? (_globalServices.currentTime
                                  .isAfter(chl.expireDate!)
                              ? -1
                              : 0)
                          : 0);

                  String remainingTime = chl.expireDate != null
                      ? "Expires in ${chl.expireDate!.difference(_globalServices.currentTime).inDays} days"
                      : "Permanent";

                  double barSize = chl.completionPercentage;
                  late Color barColor;
                  Color bgColor = Colors.blueGrey.shade100;
                  Color fontColor = Colors.white;
                  Icon? statusIcon;

                  switch (status) {
                    case 1:
                      barColor = Colors.green;
                      statusIcon = Icon(
                        Icons.check_circle_outlined,
                        size: 25,
                        color: Colors.white,
                      );
                      break;
                    case -1:
                      remainingTime = "Expired";
                      barColor = Colors.red.shade300;
                      statusIcon = Icon(
                        Icons.warning_amber,
                        size: 25,
                      );
                      barSize = 100.0;
                      break;
                    default:
                      bgColor = backgroundColors[
                          (chl.challengeId - 1) % backgroundColors.length];
                      barColor =
                          barColors[(chl.challengeId - 1) % barColors.length];
                  }

                  /// Due to the color used, challenge 6 needs to use black text
                  if (chl.challengeId == 6 && status == 0) {
                    fontColor = Colors.black;
                  }

                  return Padding(
                    padding: const EdgeInsets.all(8.0),
                    child: Column(
                      children: <Widget>[
                        GestureDetector(
                          onTap: () {
                            // User completed the challenge or challenge expired
                            if (status != 0) {
                              ApiService.delete('api/UserChallenges/${chl.id}')
                                  .then((_) => {
                                        if (status == 1)
                                          {_displayChallengeCompletion(chl)}
                                        else
                                          {_displayChallengeExpired(chl)},
                                        _initializeUser(),
                                      });
                            } else {
                              _checkChallenge(chl);
                            }
                          },
                          child: Stack(
                            children: [
                              TweenAnimationBuilder<double>(
                                duration: const Duration(seconds: 1),
                                curve: Curves.ease,
                                tween: Tween<double>(
                                  begin: 0.0,
                                  end: barSize,
                                ),
                                builder: (context, value, _) =>
                                    LinearProgressIndicator(
                                  borderRadius: BorderRadius.circular(4),
                                  backgroundColor: bgColor,
                                  color: barColor,
                                  value: value,
                                  minHeight: 50,
                                ),
                              ),
                              Positioned(
                                left: 8,
                                top: 5,
                                child: Text(
                                  chl.challengeName,
                                  style: TextStyle(
                                    fontSize: 22,
                                    color: fontColor,
                                  ),
                                ),
                              ),
                              Positioned(
                                left: 8,
                                top: 30,
                                child: Text(
                                  remainingTime,
                                  style: TextStyle(
                                    fontSize: 12,
                                    color: fontColor,
                                    fontWeight: FontWeight.bold,
                                  ),
                                ),
                              ),
                              Positioned(
                                right: 10,
                                top: 13,
                                child: Row(
                                  children: [
                                    if (statusIcon != null)
                                      Padding(
                                        padding: EdgeInsets.only(right: 5),
                                        child: statusIcon,
                                      ),
                                    Text(
                                      "${(chl.completionPercentage * 100).toInt()}%",
                                      style: TextStyle(
                                        fontSize: 20,
                                        color: fontColor,
                                        fontWeight: FontWeight.bold,
                                      ),
                                    ),
                                  ],
                                ),
                              ),
                            ],
                          ),
                        ),
                      ],
                    ),
                  );
                } else {
                  return Padding(
                    padding: const EdgeInsets.all(8.0),
                    child: Container(
                      width: double.infinity,
                      height: 50,
                      decoration: BoxDecoration(
                        border: Border.all(color: Colors.grey),
                        borderRadius: BorderRadius.circular(8.0),
                      ),
                      child: Center(
                        child: Text(
                          "Empty Challenge Slot",
                          style: const TextStyle(
                            fontSize: 18,
                            color: Colors.grey,
                          ),
                        ),
                      ),
                    ),
                  );
                }
              }),
            ),
            Expanded(
                child: Align(
              alignment: FractionalOffset.bottomCenter,
              child: Padding(
                  padding: const EdgeInsets.only(bottom: 20),
                  child: SizedBox(
                    width: 290,
                    height: 50,
                    child: FilledButton(
                        onPressed: () => toggleSleep(),
                        style: FilledButton.styleFrom(
                            backgroundColor: const Color(0xff6750a4)),
                        child: Text(
                          isSleeping ? "End Sleep" : "Start Sleeping",
                          style: const TextStyle(
                            fontSize: 30,
                          ),
                        )),
                  )),
            )),
          ],
        ),
      ),
    );
  }

  Future<void> scheduleDevices() async {
    ApiService.post('api/DeviceScheduling?UserId=$userId',
        _globalServices.currentTime.toIso8601String());
  }

  Future<void> submitSleepData(
      Survey? survey, WearableLog? wearableData) async {
    if (survey != null && wearableData != null) {
      Map<String, dynamic> payload = {
        "survey": survey.toJson(),
        "wearableData": wearableData.toJson(),
        "appTime": _globalServices.currentTime.toIso8601String()
      };

      dynamic response = await ApiService.post(
          'api/SleepReviews/GenerateReview/$userId', payload);

      if (response != null) {
        SleepReview review = SleepReview.fromJson(response);
        _popupReview(review);
        scheduleDevices();
        List<UserChallenge> newChallengeProgress =
            await _fetchChallenges(userId);
        setState(() {
          _sleepScore = review.smarterSleepScore;
          userChallenges = newChallengeProgress;
        });
      }
    }
  }

  Future<void> toggleSleep() async {
    final navigator = Navigator.of(context);
    int duration = sleepTime.elapsed.inMinutes;

    setState(() {
      isSleeping = !isSleeping;
      if (isSleeping) {
        sleepTime.start();
      } else {
        sleepTime.stop();
        sleepTime.reset();
      }
    });

    if (!isSleeping) {
      bool? userSelection = await showDialog(
        context: context,
        builder: (BuildContext context) {
          return AlertDialog(
            title: const Text('Simulated Wearable Data'),
            content: const Text(
                "For prototype purposes, would you like to simulate better or worse wearable data for this session?"),
            actions: <Widget>[
              ElevatedButton.icon(
                onPressed: () {
                  Navigator.pop(context, false);
                },
                icon: const Icon(Icons.sentiment_very_dissatisfied),
                label: const Text('Worse'),
                style: ElevatedButton.styleFrom(
                  backgroundColor: Colors.red,
                ),
              ),
              ElevatedButton.icon(
                onPressed: () {
                  Navigator.pop(context, true);
                },
                icon: const Icon(Icons.sentiment_very_satisfied),
                label: const Text('Better'),
                style: ElevatedButton.styleFrom(
                  backgroundColor: Colors.green,
                ),
              ),
            ],
          );
        },
      );
      if (userSelection == null) {
        return;
      }
      //Use wearable data to calculate sleep duration if it exists, otherwise use timer
      WearableLog? wearableData = await _fetchWearableData(userSelection);
      int minutesSlept = wearableData != null
          ? wearableData.sleepEnd.difference(wearableData.sleepStart).inMinutes
          : duration;

      Survey? survey = await navigator.push(
        //context,
        MaterialPageRoute(
          builder: (context) => SurveyForm(trackedTime: minutesSlept),
        ),
      );
      submitSleepData(survey, wearableData);
    }
  }

  void _displayChallengeCompletion(UserChallenge userChallenge) {
    showDialog(
      context: context,
      builder: (BuildContext context) {
        return AlertDialog(
          title: Text('Congratulations!'),
          content: Text(
              'You completed the ${userChallenge.challengeName} challenge! Get assigned another by completing a sleep cycle.'),
          actions: [
            TextButton(
              onPressed: () {
                Navigator.of(context).pop();
              },
              child: Text('OK'),
            ),
          ],
        );
      },
    );
  }

  void _displayChallengeExpired(UserChallenge userChallenge) {
    showDialog(
      context: context,
      builder: (BuildContext context) {
        return AlertDialog(
          title: Text('Challenge Expired'),
          content: Text(
              'You were unable to complete this challenge, to get a new one complete another sleep cycle.'),
          actions: [
            TextButton(
              onPressed: () {
                Navigator.of(context).pop();
              },
              child: Text('OK'),
            ),
          ],
        );
      },
    );
  }

  void _checkChallenge(UserChallenge userChallenge) {
    showDialog(
      context: context,
      builder: (BuildContext context) {
        return AlertDialog(
          title: Text(userChallenge.challengeName),
          content: Text(
              "${userChallenge.challengeDesc ?? "No description"}\n\nCompleted: ${userChallenge.numCompleted} | Required: ${userChallenge.numTargeted}"),
          actions: [
            TextButton(
              onPressed: () {
                Navigator.of(context).pop();
              },
              child: Text('OK'),
            ),
          ],
        );
      },
    );
  }
}
