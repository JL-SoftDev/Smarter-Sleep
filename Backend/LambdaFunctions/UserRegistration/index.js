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
        VALUES ($1, $2, $3, $4)`;

    const values = [userId, username, createdAt, 100];

    try {
        await pool.query(query, values);
        callback(null, event);
    } catch (error) {
        console.error('Error inserting data:', error);
        callback(error);
    } finally {
        pool.release(true);
        callback(null, event);
    }
};
