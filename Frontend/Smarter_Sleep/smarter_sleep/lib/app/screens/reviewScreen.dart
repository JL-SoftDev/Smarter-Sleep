import 'package:amplify_flutter/amplify_flutter.dart';
import 'package:flutter/material.dart';
import 'package:smarter_sleep/app/api/api_service.dart';
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
        title: const Text('Sleep Review'),
      ),
      body: sleepReviews.isEmpty
          ? const Center(
              child: Text('No reviews available, complete a sleep cycle first'),
            )
          : ListView.builder(
              itemCount: sleepReviews.length,
              itemBuilder: (context, index) {
                SleepReview review = sleepReviews[index];
                final sleepScore = review.smarterSleepScore;
                final sleepScoreColor = ColorUtils.getScoreColor(sleepScore);
                final sleepIcon = getSleepIcon(sleepScore);

                return Card(
                  margin: const EdgeInsets.all(8.0),
                  elevation: 4,
                  child: Container(
                    padding: const EdgeInsets.all(16),
                    child: ListTile(
                      title: Text('Sleep Date: ${review.survey.surveyDate}'),
                      leading:
                          Icon(sleepIcon, color: sleepScoreColor, size: 36),
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
    );
  }
}
