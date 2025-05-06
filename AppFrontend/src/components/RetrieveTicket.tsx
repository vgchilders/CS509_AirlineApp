import React, { useState } from 'react';
import axios from 'axios';
import { paths } from '../types/api';

function RetrieveTicket() {
  const [email, setEmail] = useState('');
  const [lastName, setLastName] = useState('');
  const [confirmationCode, setConfirmationCode] = useState('');
  const [message, setMessage] = useState<string | null>(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await axios.post('http://localhost:5000/api/v1/SendTicket/send-ticket', {
        email,
        lastName,
        confirmationCode
      } as paths['/api/v1/SendTicket/send-ticket']['post']['requestBody']['content']['application/json']);
      setMessage('Ticket has been sent to your email.');
    } catch (err) {
      console.error(err);
      setMessage('Error retrieving ticket. Please check your details and try again.');
    }
  };

  return (
    <div style={{ maxWidth: 400, margin: 'auto' }}>
      <h1>Retrieve Your Ticket</h1>
      <form onSubmit={handleSubmit}>
        <div>
          <label>Email</label>
          <input type="email" required value={email} onChange={e => setEmail(e.target.value)} />
        </div>
        <div>
          <label>Last Name</label>
          <input required value={lastName} onChange={e => setLastName(e.target.value)} />
        </div>
        <div>
          <label>Confirmation Code</label>
          <input required value={confirmationCode} onChange={e => setConfirmationCode(e.target.value)} />
        </div>
        <button type="submit">Send Ticket</button>
      </form>
      {message && <p style={{ marginTop: '1rem' }}>{message}</p>}
    </div>
  );
}

export default RetrieveTicket;