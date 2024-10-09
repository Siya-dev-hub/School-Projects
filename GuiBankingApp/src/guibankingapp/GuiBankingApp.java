/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package guibankingapp;

/**
 *
 * @author User
 */
import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.sql.*;
import java.util.ArrayList;
import java.util.List;

public class GuiBankingApp {
    
    private JFrame frame;
    private JTextField usernameField;
    private JPasswordField passwordField;
    private JTextField amountField;
    private JTextField recipientAccountField;
    private JLabel balanceLabel;
    private JLabel accountNumberLabel;
    private JLabel accountNameLabel;
    private double balance = 0.0;
    private String accountNumber;
    private String accountName;
    private int userId;
    private Connection conn;
    
    public static void main(String[] args) {
        EventQueue.invokeLater(() -> {
            try {
                GuiBankingApp window = new GuiBankingApp();
                window.frame.setVisible(true);
            } catch (Exception e) {
                e.printStackTrace();
            }
        });
    }

    public GuiBankingApp() {
        connectToDatabase();
        showLoginScreen();
    }

    private void connectToDatabase() {
        try {
            conn = DriverManager.getConnection("jdbc:mysql://localhost:3006/BankingDB", "root", "3ScaryGames"); 
        } catch (SQLException e) {
            e.printStackTrace();
            JOptionPane.showMessageDialog(frame, "Database connection failed!");
        }
    }

    private void showLoginScreen() {
        frame = new JFrame();
        frame.setBounds(100, 100, 300, 200);
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        frame.getContentPane().setLayout(null);

        JLabel lblUsername = new JLabel("Username:");
        lblUsername.setBounds(20, 20, 80, 25);
        frame.getContentPane().add(lblUsername);

        usernameField = new JTextField();
        usernameField.setBounds(100, 20, 150, 25);
        frame.getContentPane().add(usernameField);

        JLabel lblPassword = new JLabel("Password:");
        lblPassword.setBounds(20, 60, 80, 25);
        frame.getContentPane().add(lblPassword);

        passwordField = new JPasswordField();
        passwordField.setBounds(100, 60, 150, 25);
        frame.getContentPane().add(passwordField);

        JButton btnLogin = new JButton("Login");
        btnLogin.setBounds(20, 100, 100, 25);
        frame.getContentPane().add(btnLogin);

        JButton btnCreateAccount = new JButton("Create Account");
        btnCreateAccount.setBounds(130, 100, 120, 25);
        frame.getContentPane().add(btnCreateAccount);

        btnLogin.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                String username = usernameField.getText();
                String password = new String(passwordField.getPassword());
                if (authenticateUser(username, password)) {
                    JOptionPane.showMessageDialog(frame, "Login successful!");
                    frame.dispose();
                    initialize();
                } else {
                    JOptionPane.showMessageDialog(frame, "Invalid username or password!");
                }
            }
        });

        btnCreateAccount.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                String username = usernameField.getText();
                String password = new String(passwordField.getPassword());
                createUserAccount(username, password);
            }
        });

        frame.setVisible(true);
    }

    private boolean authenticateUser(String username, String password) {
        try {
            String sql = "SELECT id, account_number, account_name, balance FROM users WHERE username=? AND password=?";
            PreparedStatement pstmt = conn.prepareStatement(sql);
            pstmt.setString(1, username);
            pstmt.setString(2, password);
            ResultSet rs = pstmt.executeQuery();
            if (rs.next()) {
                userId = rs.getInt("id");
                accountNumber = rs.getString("account_number");
                accountName = rs.getString("account_name");
                balance = rs.getDouble("balance");
                return true;
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }
        return false;
    }

    private void createUserAccount(String username, String password) {
        try {
            String sql = "INSERT INTO users (username, password, account_number, account_name, balance) VALUES (?, ?, ?, ?, ?)";
            PreparedStatement pstmt = conn.prepareStatement(sql);
            pstmt.setString(1, username);
            pstmt.setString(2, password);
            pstmt.setString(3, generateAccountNumber());
            pstmt.setString(4, username); // For simplicity, using username as account name
            pstmt.setDouble(5, 0.0);
            pstmt.executeUpdate();
            JOptionPane.showMessageDialog(frame, "Account created successfully!");
        } catch (SQLException e) {
            e.printStackTrace();
            JOptionPane.showMessageDialog(frame, "Failed to create account!");
        }
    }

    private String generateAccountNumber() {
        // Implement account number generation logic here
        return String.valueOf(System.currentTimeMillis()); // Simple placeholder
    }

    private void initialize() {
    frame = new JFrame();
    frame.setBounds(100, 100, 450, 450);
    frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
    frame.getContentPane().setLayout(null);

    JLabel lblAmount = new JLabel("Amount:");
    lblAmount.setBounds(50, 100, 80, 25);
    frame.getContentPane().add(lblAmount);

    amountField = new JTextField();
    amountField.setBounds(150, 100, 200, 25);
    frame.getContentPane().add(amountField);
    amountField.setColumns(10);

    JLabel lblRecipientAccount = new JLabel("Recipient Account:");
    lblRecipientAccount.setBounds(50, 150, 150, 25);
    frame.getContentPane().add(lblRecipientAccount);

    recipientAccountField = new JTextField();
    recipientAccountField.setBounds(200, 150, 200, 25);
    frame.getContentPane().add(recipientAccountField);
    recipientAccountField.setColumns(10);

    JButton btnDeposit = new JButton("Deposit");
    btnDeposit.setBounds(50, 200, 100, 25);
    frame.getContentPane().add(btnDeposit);

    JButton btnWithdraw = new JButton("Withdraw");
    btnWithdraw.setBounds(250, 200, 100, 25);
    frame.getContentPane().add(btnWithdraw);

    JButton btnTransfer = new JButton("Transfer");
    btnTransfer.setBounds(150, 250, 100, 25);
    frame.getContentPane().add(btnTransfer);

    JButton btnTransactionHistory = new JButton("Transaction History");
    btnTransactionHistory.setBounds(150, 300, 150, 25);
    frame.getContentPane().add(btnTransactionHistory);

    balanceLabel = new JLabel("Balance: $" + balance);
    balanceLabel.setBounds(50, 350, 300, 25);
    frame.getContentPane().add(balanceLabel);

    accountNumberLabel = new JLabel("Account Number: " + accountNumber);
    accountNumberLabel.setBounds(50, 20, 300, 25);
    frame.getContentPane().add(accountNumberLabel);

    accountNameLabel = new JLabel("Account Name: " + accountName);
    accountNameLabel.setBounds(50, 50, 300, 25);
    frame.getContentPane().add(accountNameLabel);

    // Add a Logout button
    JButton btnLogout = new JButton("Logout");
    btnLogout.setBounds(300, 350, 100, 25);
    frame.getContentPane().add(btnLogout);

    // Action listeners
    btnDeposit.addActionListener(new ActionListener() {
        public void actionPerformed(ActionEvent e) {
            double amount = Double.parseDouble(amountField.getText());
            balance += amount;
            updateBalance();
            recordTransaction("Deposit", amount, null);
        }
    });

    btnWithdraw.addActionListener(new ActionListener() {
        public void actionPerformed(ActionEvent e) {
            double amount = Double.parseDouble(amountField.getText());
            if (amount <= balance) {
                balance -= amount;
                updateBalance();
                recordTransaction("Withdraw", amount, null);
            } else {
                JOptionPane.showMessageDialog(frame, "Insufficient funds!");
            }
        }
    });

    btnTransfer.addActionListener(new ActionListener() {
    public void actionPerformed(ActionEvent e) {
        double amount = Double.parseDouble(amountField.getText());
        String recipientAccount = recipientAccountField.getText();

        if (amount <= balance) {
            try {
                conn.setAutoCommit(false); // Start transaction

                // Deduct amount from sender's balance
                balance -= amount;
                updateBalance(); // Update sender's balance in the database

                // Add amount to recipient's balance
                String sql = "UPDATE users SET balance = balance + ? WHERE account_number = ?";
                PreparedStatement pstmt = conn.prepareStatement(sql);
                pstmt.setDouble(1, amount);
                pstmt.setString(2, recipientAccount);
                int rowsAffected = pstmt.executeUpdate();

                if (rowsAffected > 0) {
                    // Record the transaction
                    recordTransaction("Transfer", amount, recipientAccount);
                    JOptionPane.showMessageDialog(frame, "Transferred $" + amount + " to account " + recipientAccount);

                    conn.commit(); // Commit the transaction
                } else {
                    conn.rollback(); // Rollback if recipient account does not exist
                    JOptionPane.showMessageDialog(frame, "Recipient account not found!");
                    balance += amount; // Revert sender's balance
                    updateBalance(); // Update the reverted balance in the database
                }
            } catch (SQLException ex) {
                try {
                    conn.rollback(); // Rollback on error
                } catch (SQLException rollbackEx) {
                    rollbackEx.printStackTrace();
                }
                ex.printStackTrace();
                JOptionPane.showMessageDialog(frame, "Transfer failed!");
            } finally {
                try {
                    conn.setAutoCommit(true); // Reset autocommit
                } catch (SQLException resetEx) {
                    resetEx.printStackTrace();
                }
            }
        } else {
            JOptionPane.showMessageDialog(frame, "Insufficient funds!");
        }
    }
});


    btnTransactionHistory.addActionListener(new ActionListener() {
        public void actionPerformed(ActionEvent e) {
            showTransactionHistory();
        }
    });

    btnLogout.addActionListener(new ActionListener() {
        public void actionPerformed(ActionEvent e) {
            frame.dispose();
            showLoginScreen();
        }
    });

    frame.setVisible(true);
}


    private void updateBalance() {
        balanceLabel.setText("Balance: $" + balance);
        try {
            String sql = "UPDATE users SET balance=? WHERE id=?";
            PreparedStatement pstmt = conn.prepareStatement(sql);
            pstmt.setDouble(1, balance);
            pstmt.setInt(2, userId);
            pstmt.executeUpdate();
        } catch (SQLException e) {
            e.printStackTrace();
        }
    }

    private void recordTransaction(String type, double amount, String recipientAccount) {
        try {
            String sql = "INSERT INTO transactions (account_number, type, amount, recipient_account) VALUES (?, ?, ?, ?)";
            PreparedStatement pstmt = conn.prepareStatement(sql);
            pstmt.setString(1, accountNumber);
            pstmt.setString(2, type);
            pstmt.setDouble(3, amount);
            pstmt.setString(4, recipientAccount);
            pstmt.executeUpdate();
        } catch (SQLException e) {
            e.printStackTrace();
        }
    }

    private void showTransactionHistory() {
        try {
            String sql = "SELECT * FROM transactions WHERE account_number=?";
            PreparedStatement pstmt = conn.prepareStatement(sql);
            pstmt.setString(1, accountNumber);
            ResultSet rs = pstmt.executeQuery();
            List<String> transactions = new ArrayList<>();
            while (rs.next()) {
                String type = rs.getString("type");
                double amount = rs.getDouble("amount");
                String recipient = rs.getString("recipient_account");
                String timestamp = rs.getTimestamp("timestamp").toString();
                String transaction = String.format("%s: $%.2f %s %s", type, amount, (recipient != null ? "to " + recipient : ""), timestamp);
                transactions.add(transaction);
            }
            String history = String.join("\n", transactions);
            JOptionPane.showMessageDialog(frame, history.isEmpty() ? "No transactions yet." : history,
                    "Transaction History", JOptionPane.INFORMATION_MESSAGE);
        } catch (SQLException e) {
            e.printStackTrace();
        }
    }
}
