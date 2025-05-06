import React from 'react';
import { Link } from 'react-router-dom';

function PaymentCancel() {
  return (
    <div style={{ maxWidth: 600, margin: 'auto', textAlign: 'center' }}>
      <h1>Payment Cancelled</h1>
      <p>Your payment was not completed. You can retry or contact support if you need assistance.</p>
      <Link to="/">Return to Home</Link>
    </div>
  );
}

export default PaymentCancel;