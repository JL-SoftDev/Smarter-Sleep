class UserChallenge {
  final int id;
  final int challengeId;
  final String challengeName;
  final String? challengeDesc;
  final DateTime startDate;
  final DateTime? expireDate;
  final bool userSelected;
  final double completionPercentage;
  final int numCompleted;
  final int numTargeted;

  UserChallenge({
    required this.id,
    required this.challengeId,
    required this.challengeName,
    this.challengeDesc,
    required this.startDate,
    this.expireDate,
    required this.userSelected,
    required this.completionPercentage,
    required this.numCompleted,
    required this.numTargeted,
  });
  factory UserChallenge.fromJson(Map<String, dynamic> json) {
    return UserChallenge(
      id: json['challengeLogId'],
      challengeId: json['challengeId'],
      challengeName: json['challengeName'],
      challengeDesc: json['challengeDescription'],
      startDate: DateTime.parse(json['startDate']),
      expireDate: DateTime.tryParse(json['expireDate'] ?? ''),
      userSelected: json['userSelected'],
      completionPercentage: json['completionPercentage'] / 1.0,
      numCompleted: json['completed'],
      numTargeted: json['goal'],
    );
  }
}
