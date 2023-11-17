import 'package:flutter/material.dart';
import 'package:fl_chart/fl_chart.dart';

import 'package:smarter_sleep/app/models/sleep_review.dart';

class SleepInsightScreen extends StatefulWidget {
  final SleepReview review;

  const SleepInsightScreen({super.key, required this.review});

  @override
  State<SleepInsightScreen> createState() => _SleepInsightScreenState();
}

class _SleepInsightScreenState extends State<SleepInsightScreen> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Sleep Insight - ${widget.review.survey.surveyDate}'),
      ),
      body: Column(
        children: [
          SizedBox(
            height: MediaQuery.of(context).size.height * 0.28,
            child: LineChart(
              LineChartData(
                lineBarsData: [
                  LineChartBarData(
                    spots: widget.review.wearableLog.hypnogram
                        .split('')
                        .asMap()
                        .map((key, char) => MapEntry(
                            key, FlSpot(key.toDouble(), double.parse(char))))
                        .values
                        .toList(),
                    isCurved: false,
                    color: Colors.blue,
                    dotData: const FlDotData(show: false),
                    belowBarData: BarAreaData(show: false),
                  ),
                ],
                borderData: FlBorderData(
                  show: true,
                  border: Border.all(
                    color: const Color(0xff37434d),
                    width: 1,
                  ),
                ),
              ),
            ),
          ),
        ],
      ),
    );
  }
}
