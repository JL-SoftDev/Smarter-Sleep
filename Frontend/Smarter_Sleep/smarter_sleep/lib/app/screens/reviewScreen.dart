import 'package:amplify_flutter/amplify_flutter.dart';
import 'package:fl_chart/fl_chart.dart';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:smarter_sleep/app/api/api_service.dart';
import 'package:smarter_sleep/app/appFrame.dart';
import 'package:smarter_sleep/app/models/sleep_review.dart';
import 'package:smarter_sleep/app/utils/color_utils.dart';

import 'sleepInsightScreen.dart';

class SleepReviewScreen extends StatefulWidget {
  const SleepReviewScreen({super.key});

  @override
  State<StatefulWidget> createState() => _SleepReviewScreenState();
}

class _SleepReviewScreenState extends State<SleepReviewScreen> {
  List<SleepReview> sleepReviews = [];

  @override
  void initState() {
    super.initState();
    fetchReviews();
  }

  Future<void> fetchReviews() async {
    dynamic response = await ApiService.get('api/SleepReviews');

    if (response != null) {
      final user = await Amplify.Auth.getCurrentUser();

      List<SleepReview> fetchedReviews = response
          .where((reviewData) =>
              reviewData['userId'] == user.userId &&
              reviewData['survey'] != null &&
              reviewData['wearableLog'] != null)
          .map<SleepReview>((reviewData) {
        return SleepReview.fromJson(reviewData);
      }).toList();

      fetchedReviews.sort((a, b) => a.createdAt.compareTo(b.createdAt));
      setState(() {
        sleepReviews = fetchedReviews;
      });
    }
  }

  IconData getSleepIcon(int sleepScore) {
    if (sleepScore < 25) {
      return Icons.sentiment_very_dissatisfied;
    } else if (sleepScore >= 25 && sleepScore <= 50) {
      return Icons.sentiment_dissatisfied;
    } else if (sleepScore > 50 && sleepScore <= 75) {
      return Icons.sentiment_satisfied;
    } else {
      return Icons.sentiment_very_satisfied;
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text("Sleep Review"),
        actions: [
          IconButton(
            icon: const Icon(Icons.account_circle),
            onPressed: () {
              mainNavigatorKey.currentState!.pushNamed("/account");
            },
          ),
        ],
      ),
      body: Column(
        children: [
          SizedBox(
            height: 20,
          ),
          Text(
            'Smarter Sleep Score Graph',
            style: TextStyle(
              color: Colors.blue[800],
              fontSize: 18,
              fontWeight: FontWeight.bold,
              letterSpacing: 2,
            ),
            textAlign: TextAlign.center,
          ),
          Padding(
            padding: const EdgeInsets.only(right: 16, left: 6, top: 15),
            child: Center(
              child: SizedBox(
                height: MediaQuery.of(context).size.height * 0.22,
                width: MediaQuery.of(context).size.width * 0.90,
                child: LineChart(sleepScores()),
              ),
            ),
          ),
          SizedBox(height: 10),
          Text(
            'Recorded Insights',
            style: TextStyle(
              color: Colors.blue[800],
              fontSize: 18,
              fontWeight: FontWeight.bold,
              letterSpacing: 2,
            ),
            textAlign: TextAlign.center,
          ),
          SizedBox(height: 10),
          Expanded(
            child: sleepReviews.isEmpty
                ? const Center(
                    child: Text(
                        'No reviews available, complete a sleep cycle first'),
                  )
                : ListView.builder(
                    itemCount: sleepReviews.length,
                    itemBuilder: (context, index) {
                      SleepReview review = sleepReviews[index];
                      final sleepScore = review.smarterSleepScore;
                      final sleepScoreColor =
                          ColorUtils.getScoreColor(sleepScore);
                      final sleepIcon = getSleepIcon(sleepScore);

                      return Card(
                        margin: const EdgeInsets.all(8.0),
                        elevation: 4,
                        child: Container(
                          padding: const EdgeInsets.all(16),
                          child: ListTile(
                            title: Text(
                                'Sleep Date: ${DateFormat('MMM dd, yyyy').format(review.createdAt)}'),
                            leading: Icon(sleepIcon,
                                color: sleepScoreColor, size: 36),
                            trailing: Stack(
                              alignment: Alignment.center,
                              children: <Widget>[
                                Transform.scale(
                                  scale: 0.8,
                                  child: CircularProgressIndicator(
                                    value: sleepScore / 100,
                                    valueColor: AlwaysStoppedAnimation<Color>(
                                        sleepScoreColor),
                                  ),
                                ),
                                Text('$sleepScore',
                                    style: TextStyle(
                                        fontSize: 14, color: sleepScoreColor)),
                              ],
                            ),
                            onTap: () {
                              Navigator.push(
                                context,
                                MaterialPageRoute(
                                  builder: (context) =>
                                      SleepInsightScreen(review: review),
                                ),
                              );
                            },
                          ),
                        ),
                      );
                    },
                  ),
          )
        ],
      ),
    );
  }

  LineChartData sleepScores() {
    List<FlSpot> data = sleepReviews
        .map((review) => FlSpot(
            review.createdAt.millisecondsSinceEpoch.toDouble(),
            review.smarterSleepScore.toDouble()))
        .toList();

    return LineChartData(
      gridData: const FlGridData(show: false),
      titlesData: FlTitlesData(
        show: true,
        rightTitles: const AxisTitles(
          sideTitles: SideTitles(showTitles: false),
        ),
        topTitles: const AxisTitles(
          sideTitles: SideTitles(showTitles: false),
        ),
        bottomTitles: AxisTitles(
          sideTitles: SideTitles(
            showTitles: true,
            interval: 86400000,
            getTitlesWidget: (double value, TitleMeta meta) {
              final dateTime =
                  DateTime.fromMillisecondsSinceEpoch(value.toInt());
              if (value == meta.min) {
                return Text("");
              }
              return Text(DateFormat('MM/dd').format(dateTime));
            },
          ),
        ),
        leftTitles: AxisTitles(
          sideTitles: SideTitles(
            showTitles: true,
            interval: 25,
            getTitlesWidget: (double value, TitleMeta meta) {
              if (value > 100) {
                return Text("");
              }
              return Text(value.toInt().toString());
            },
            reservedSize: 30,
          ),
        ),
      ),
      minY: 0,
      maxY: 105,
      lineBarsData: [
        LineChartBarData(
          spots: data,
          isCurved: true,
          preventCurveOverShooting: true,
          isStrokeCapRound: true,
          color: Colors.blue[800],
          dotData: const FlDotData(show: false),
          gradient: LinearGradient(
            colors: [Color(0xFF50E4FF), Color(0xFF2196F3)],
          ),
          belowBarData: BarAreaData(
              show: true,
              gradient: LinearGradient(
                colors: [
                  Color(0xFF50E4FF).withOpacity(0.3),
                  Color(0xFF2196F3).withOpacity(0.3),
                ],
              )),
        ),
      ],
      borderData: FlBorderData(
        show: true,
        border: Border.all(
          color: const Color(0xff37434d),
          width: 1,
        ),
      ),
    );
  }
}
