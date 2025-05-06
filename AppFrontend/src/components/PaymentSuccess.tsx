import React from 'react';
import { useSearchParams, Link } from 'react-router-dom';

function PaymentSuccess() {
  const [searchParams] = useSearchParams();
  const sessionId = searchParams.get('session_id');

  return (
    <div style={{ maxWidth: 600, margin: 'auto', textAlign: 'center' }}>
      <h1>Payment Successful</h1>
      <p>Thank you for your purchase!</p>
      {/* {sessionId && <p>Your payment session ID: <code>{sessionId}</code></p>} */}
      <p>Your ticket confirmation and receipt will be emailed to you shortly.</p>
      <Link to="/">Back to Home</Link>
    </div>
  );
}

export default PaymentSuccess;