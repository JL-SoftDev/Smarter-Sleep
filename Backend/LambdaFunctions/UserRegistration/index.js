const { Pool } = require('pg');

const pool = new Pool({
    connectionString: process.env.DB_CONNECTION
});

exports.handler = async (event, context, callback) => {
    console.info("EVENT\n" + JSON.stringify(event, null, 2))
    context.callbackWaitsForEmptyEventLoop = false;
    const username = event.userName;
    const userId = event.request.userAttributes.sub;
    const createdAt = new Date().toISOString();

    const query = `
        INSERT INTO app_user (user_id, username, created_at, points)
        VALUES ($1, $2, $3, $4)
        ON CONFLICT (user_id) DO UPDATE
        SET username = $2`;

    const values = [userId, username, createdAt, 100];

    try {
        await pool.query(query, values);
        // First time signup
        if(event.triggerSource == "PostConfirmation_ConfirmSignUp"){
            // Assign challenge to set wake up schedule
            await pool.query(`
                INSERT INTO user_challenge (user_id, challenge_id, completed, start_date, user_selected)
                VALUES ($1, 6, FALSE, CURRENT_TIMESTAMP, FALSE)
                ON CONFLICT (user_id, challenge_id) DO NOTHING`, [userId]);
        }
    } catch (error) {
        console.error('Error inserting data:', error);
        callback(error);
    } finally {
        callback(null, event);
    }
};
