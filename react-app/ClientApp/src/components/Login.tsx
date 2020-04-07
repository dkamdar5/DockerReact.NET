import React, { useState } from 'react';
import { Link } from 'react-router-dom';

export default () => {
  const [values, setValues] = useState({username: "", password: ""})

  const handleInputChange = (e) => {
    const target = e.target;
    const value = target.value;
    const name = target.name;

    setValues({
      ...values,
      [name]: value
    })
  }

  const handleSubmit = async(e) => {
    e.preventDefault();

    var formData = JSON.stringify({ "username": values.username, "password": values.password });
    const response = await fetch("/Home/Login", {
      method: 'post',
      body: formData,
      headers: { "Content-Type": "application/json" },
      credentials: 'include'
    })
  }

  return (
    <div>
      <h1>Login</h1>

      <form>
        <div>
          <label>Username: </label>
          <input type="text" name="username" value={values.username} onChange={handleInputChange} />
        </div>
        <div>
          <label>Password: </label>
          <input type="password" name="password" value={values.password} onChange={handleInputChange} />
        </div>
        <button className="btn btn-primary" type="button" onClick={handleSubmit}>Login</button>
      </form>
      <Link to='/register'>Register</Link>
    </div>
  );
}
